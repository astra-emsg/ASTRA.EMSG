using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ASTRA.EMSG.IntegrationTests.Support.MvcTesting
{
    public static class NameValueCollectionExtensions
    {
        public static void AddToCollection<TModel, TCollection, TElement>(this NameValueCollection nameValueCollection, Expression<Func<TModel, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TCollection, TElement>> memberSelector, TCollection value)
        {
            var collectionName = collectionSelector.GetPropertyName();
            var memberName = memberSelector.GetPropertyName();
            var idProperty = typeof(TCollection).GetProperties().Single(p => p.Name == "Id");
            string id = idProperty.GetValue(value, null).ToString();
            nameValueCollection.Add(string.Format("{0}.Index", collectionName), id);
            nameValueCollection.Add(string.Format("{0}[{1}].Id", collectionName, id), id);
            nameValueCollection.Add(string.Format("{0}[{1}].{2}", collectionName, id, memberName), memberSelector.Compile()(value).ToString());
        }

        public static void RemoveFromCollection<TObject, TCollection>(this NameValueCollection nameValueCollection, Expression<Func<TObject, IEnumerable<TCollection>>> collectionSelector, object id)
        {
            var collectionName = collectionSelector.GetPropertyName();
            var entriesToRemove = nameValueCollection.AllKeys.Where(k => k.Contains(collectionName) && k.Contains(id.ToString())).ToArray();
            foreach (var entry in entriesToRemove)
            {
                nameValueCollection.Remove(entry);
            }
            string indexKey = string.Format("{0}.Index", collectionName);
            var indexes = nameValueCollection.Get(indexKey).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            indexes.RemoveAll(i => i.ToLower() == id.ToString().ToLower());
            nameValueCollection.Set(indexKey, string.Join(",", indexes));
        }

        public static void RemoveFromCollection<TObject, TCollection, TElement>(this NameValueCollection nameValueCollection, Expression<Func<TObject, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TCollection, TElement>> memberSelector, TElement value)
        {
            var collectionName = collectionSelector.GetPropertyName();
            var memberName = collectionSelector.GetPropertyName();
            var keys = nameValueCollection.AllKeys.Where(k => k.Contains(collectionName) && k.Contains(memberName)).ToArray();
            foreach (var key in keys)
            {
                if (nameValueCollection.Get(key).Contains(value.ToString()))
                {
                    var id = new Regex(@".*\[(.*)\].*").Match(key).Groups[1];
                    nameValueCollection.RemoveFromCollection(collectionSelector, id.Value);
                    break;
                }
            }
        }

        public static void UpdateCollectionMember<TModel, TCollection, TKey, TValue>
        (
            this NameValueCollection nameValueCollection,
            Expression<Func<TModel, IEnumerable<TCollection>>> collectionSelector,
            Expression<Func<TCollection, object>> keySelector,
            Func<TKey, bool> matchKey,
            Expression<Func<TCollection, object>> valueSelector,
            TValue value
        )
        {
            var converter = TypeDescriptor.GetConverter(typeof(TKey));
            UpdateCollectionMember<TModel, TCollection>
            (
                nameValueCollection, collectionSelector,
                keySelector, str => matchKey((TKey)converter.ConvertFromString(str)),
                valueSelector, value.ToString()
            );
        }

        public static void UpdateCollectionMember<TModel, TCollection>
        (
            this NameValueCollection nameValueCollection,
            Expression<Func<TModel, IEnumerable<TCollection>>> collectionSelector,
            Expression<Func<TCollection, object>> keySelector, Func<string, bool> matchKey,
            Expression<Func<TCollection, object>> memberSelector, string value
        )
        {
            UpdateCollectionMember<TModel, TCollection>
            (
                nameValueCollection, collectionSelector,
                new Dictionary<Expression<Func<TCollection, object>>, Func<string, bool>> { { keySelector, matchKey } },
                memberSelector, value.ToString()
            );
        }

        public static void UpdateCollectionMember<TModel, TCollection>
        (
            this NameValueCollection nameValueCollection, Expression<Func<TModel, IEnumerable<TCollection>>> collectionSelector,
            IEnumerable<KeyValuePair<Expression<Func<TCollection, object>>, Func<string, bool>>> selectorMatcherPairs,
            Expression<Func<TCollection, object>> valueSelector, string value
        )
        {
            var collectionName = collectionSelector.GetPropertyName();
            var memberName = valueSelector.GetPropertyName();

            var allKeys = nameValueCollection.AllKeys
                .Where(key => key.StartsWith(collectionName))
                .Where(key => key.Contains('.')).ToList();
            var valueSelectors = allKeys
                .Where(key =>
                    selectorMatcherPairs.All(pair =>
                        allKeys.Any(other =>
                            key.Remove(key.LastIndexOf('.')) == other.Remove(other.LastIndexOf('.')) &&
                            other.EndsWith(pair.Key.GetPropertyName()) &&
                            pair.Value(nameValueCollection.Get(other))
                        )
                    )
                )
                .Select(key => key.Remove(key.LastIndexOf('.')) + "." + memberName)
                .Distinct();

            foreach (var key in valueSelectors)
            {
                nameValueCollection.Set(key, value.ToString());
            }
        }
    }
}