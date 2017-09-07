using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.GISExport
{
    public interface IGISExportMapPoProvider : IPoProvider
    {

    }

    [ReportInfo(AuswertungTyp.W6_1, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class GISExportMapPoProvider : EmsgGisMapPoProviderBase<GISExportParameter, GISExportMapParameter, GISExportPo>, IGISExportMapPoProvider
    {
        public GISExportMapPoProvider(IGISExportMapProvider mapProviderBase)
            : base(mapProviderBase)
        {
        }
    }
}
