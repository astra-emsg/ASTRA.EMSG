using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Common;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.GIS;
using ASTRA.EMSG.Business.Services.Historization;

namespace ASTRA.EMSG.Business.Reports.GISExport
{
    public interface IGISExportMapProvider : IMapInfoProviderBase<GISExportMapParameter>, IService
    {
    }

    public class GISExportMapProvider : MapInfoProviderBase<GISExportParameter,GISExportMapParameter,StrassenabschnittGIS>, IGISExportMapProvider
    {
        public GISExportMapProvider(IGISReportService gISReportService, ILocalizationService localizationService,
                                    IErfassungsPeriodService erfassungsPeriodService,
                                    IHistorizationService historizationService)
            : base(gISReportService, localizationService, erfassungsPeriodService, historizationService)
        {
        }
    }
}
