using System.Collections.Generic;
using System.Web.Mvc;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class FluentTagBuilder
    {
        private readonly TagBuilder mBuilder;

        public FluentTagBuilder(string tagName)
        {
            mBuilder = new TagBuilder(tagName);
        }

        public FluentTagBuilder MergeAttribute(string key, string value)
        {
            mBuilder.MergeAttribute(key, value);

            return this;
        }

        public FluentTagBuilder AddCssClass(string cssClass)
        {
            mBuilder.AddCssClass(cssClass);

            return this;
        }

        public string InnerHtml
        {
            get { return mBuilder.InnerHtml; }
            set { mBuilder.InnerHtml = value; }
        }

        public FluentTagBuilder AddToInnerHtml(FluentTagBuilder tagBuilder)
        {
            mBuilder.InnerHtml += tagBuilder.ToString(TagRenderMode.Normal);

            return this;
        }

        public FluentTagBuilder AddToInnerHtml(string html)
        {
            mBuilder.InnerHtml += html;

            return this;
        }

        public FluentTagBuilder SetInnerText(string innerText)
        {
            mBuilder.SetInnerText(innerText);

            return this;
        }

        public FluentTagBuilder GenerateId(string name)
        {
            mBuilder.GenerateId(name);

            return this;
        }

        public override string ToString()
        {
            return mBuilder.ToString();
        }

        public string ToString(TagRenderMode renderMode)
        {
            return mBuilder.ToString(renderMode);
        }

        public FluentTagBuilder AddAttribute(string attribute, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                mBuilder.Attributes.Add(new KeyValuePair<string, string>(attribute, value));

            return this;
        }
    }
}