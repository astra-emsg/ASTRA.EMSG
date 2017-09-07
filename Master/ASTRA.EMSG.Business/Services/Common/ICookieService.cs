using System;
using ASTRA.EMSG.Business.Services.Security;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Services.Common
{
    public interface ICookieService
    {
        string this[string key] { get; set; }
        EmsgLanguage? EmsgLanguage { get; set; }
        Guid? SelectedMandantId { get; set; }
        ApplicationMode? SelectedApplicationMode { get; set; }
    }
}