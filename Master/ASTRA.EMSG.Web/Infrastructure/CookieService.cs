using System;
using System.Web;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class CookieService : ICookieService
    {
        private const string EmsgLanguageKey = "EmsgLanguage";
        private const string SelectedApplicationModeKey = "SelectedApplicationMode";
        private const string SelectedMandantIdKey = "SelectedMandantId";

        public virtual string this[string key]
        {
            get
            {
                HttpCookie httpCookie = HttpContext.Current.Request.Cookies.Get(key);
                return httpCookie != null ? httpCookie.Value : null;
            }
            set
            {
                var httpCookie = new HttpCookie(key, value) {Expires = DateTime.MaxValue};
                HttpContext.Current.Response.Cookies.Set(httpCookie);
            }
        }

        public EmsgLanguage? EmsgLanguage
        {
            get { return this[EmsgLanguageKey].ParseAsEnum<EmsgLanguage>(); }
            set { this[EmsgLanguageKey] = value.HasValue ? value.Value.ToString() : null; }
        }

        public Guid? SelectedMandantId
        {
            get
            {
                string id = this[SelectedMandantIdKey];
                return string.IsNullOrEmpty(id) ? (Guid?) null : Guid.Parse(id);
            }
            set { this[SelectedMandantIdKey] = value.HasValue ? value.Value.ToString() : null; }
        }

        public ApplicationMode? SelectedApplicationMode
        {
            get { return this[SelectedApplicationModeKey].ParseAsEnum<ApplicationMode>(); }
            set { this[SelectedApplicationModeKey] = value.HasValue ? value.Value.ToString() : null; }
        }
    }
}