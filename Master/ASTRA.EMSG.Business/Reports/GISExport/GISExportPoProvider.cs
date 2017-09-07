using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Business.Services.EntityServices.Katalogs;
using ASTRA.EMSG.Business.Services.FilterBuilders;
using ASTRA.EMSG.Common.Enums;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.GISExport
{
    public interface IGISExportPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W6_1, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class GISExportPoProvider : EmsgTablePoProviderBase<GISExportParameter, GISExportPo>, IGISExportPoProvider
    {
        public GISExportPoProvider()
        {
        }
    }
}
