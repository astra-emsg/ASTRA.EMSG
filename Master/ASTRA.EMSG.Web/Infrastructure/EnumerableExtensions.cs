using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<DropDownListItem> ToDropDownItemList<T>(this IEnumerable<T> input, Func<T, string> textSelector, Func<T, string> valueSelector, object selectedItem = null, string emptyItemText = null)
        {
            var dropDownItems = new List<DropDownListItem>();
            if (emptyItemText != null)
                dropDownItems.Add(new DropDownListItem { Selected = true, Text = emptyItemText, Value = "" });
            dropDownItems.AddRange(input
                .Select(item => new DropDownListItem { Text = textSelector(item), Value = valueSelector(item), Selected = Equals(selectedItem, item) }));

            return dropDownItems;
        }

        public static IEnumerable<DropDownListItem> ToDropDownItemList<T, TTextSelector, TValueSelector>(this IEnumerable<T> input, Func<T, TTextSelector> textSelector, Func<T, TValueSelector> valueSelector, object selectedItem = null, string emptyItemText = null)
        {
            return ToDropDownItemList(input, obj => 
            { 
                var t = textSelector(obj);
                return t != null ? t.ToString() : "";
            }, obj =>
            { 
                var v = valueSelector(obj);
                return v != null ? v.ToString() : "";
            },
            selectedItem, emptyItemText);
        }
    }
}