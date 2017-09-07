using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Web.Infrastructure
{
    public class SessionService : ISessionService
    {
        public const string LastImportResultKey = "LastImportResult";
        public const string LastStrassenabschnittImportResultKey = "LastStrassenabschnittImportResult";
        public const string LastZustandsabschnittImportResultKey = "LastZustandsabschnittImportResult";
        public const string LastImportErrorListKey = "LastImportErrorList";
        public const string LastUploadErrorListKey = "LastUploadErrorList";
        public const string SelectedMandantIdKey = "SelectedMandantId";
        public const string LastGeneratedReportKey = "LastGeneratedReport";
        public const string LastGeneratedMapReportKey = "LastGeneratedMapReport";
        public const string SupportedUserInfoKey = "SupportedUserInfo";
        public const string LastExportedPackageKey = "LastExportedPackage";

        public virtual object this[string key] { get { return HttpContext.Current.Session[key]; } set { HttpContext.Current.Session[key] = value; } }

        public ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel> LastStrassenabschnittImportResult { get { return (ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel>)this[LastStrassenabschnittImportResultKey]; } set { this[LastStrassenabschnittImportResultKey] = value; } }
        public ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel> LastZustandsabschnittImportResult { get { return (ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel>)this[LastZustandsabschnittImportResultKey]; } set { this[LastZustandsabschnittImportResultKey] = value; } }
        
        public List<string> LastImportErrorList { get { return (List<string>)this[LastImportErrorListKey]; } set { this[LastImportErrorListKey] = value; } }
        public List<string> LastUploadErrorList { get { return (List<string>)this[LastUploadErrorListKey]; } set { this[LastUploadErrorListKey] = value; } }
        
        public Guid? SelectedMandantId { get { return (Guid?)this[SelectedMandantIdKey]; } set { this[SelectedMandantIdKey] = value; } }
        public Report LastGeneratedReport { get { return (Report)this[LastGeneratedReportKey]; } set { this[LastGeneratedReportKey] = value; } }
        public Report LastGeneratedMapReport { get { return (Report)this[LastGeneratedMapReportKey]; } set { this[LastGeneratedMapReportKey] = value; } }
        public Stream LastExportedPackage { get { return (Stream)this[LastExportedPackageKey]; } set { this[LastExportedPackageKey] = value; } }

        public SupportedUserInfo SupportedUserInfo { get { return (SupportedUserInfo)this[SupportedUserInfoKey]; } set { this[SupportedUserInfoKey] = value; } }

        public void Abandon()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}