using System;
using System.Collections.Generic;
using System.IO;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Models.Strassennamen;
using ASTRA.EMSG.Business.Services.Security;

namespace ASTRA.EMSG.Business.Services.Common
{
    public interface ISessionService : IStoreService
    {
        ImportResultModel<StrassenabschnittImportModel, StrassenabschnittImportOverviewModel> LastStrassenabschnittImportResult { get; set; }
        ImportResultModel<ZustandsabschnittImportModel, ZustandsabschnittImportOverviewModel> LastZustandsabschnittImportResult { get; set; }
        List<string> LastImportErrorList { get; set; }
        List<string> LastUploadErrorList { get; set; }
        Guid? SelectedMandantId { get; set; }
        Report LastGeneratedReport { get; set; }
        Report LastGeneratedMapReport { get; set; }
        Stream LastExportedPackage { get; set; }
        SupportedUserInfo SupportedUserInfo { get; set; }
        void Abandon();
    }
}
