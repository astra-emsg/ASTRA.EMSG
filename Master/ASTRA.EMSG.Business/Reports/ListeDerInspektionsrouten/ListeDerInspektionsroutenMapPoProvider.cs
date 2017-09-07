using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Utils;
using System.Linq;
using ASTRA.EMSG.Business.Services.GIS;

namespace ASTRA.EMSG.Business.Reports.ListeDerInspektionsrouten
{
    public interface IListeDerInspektionsroutenMapPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W1_3, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class ListeDerInspektionsroutenMapPoProvider : EmsgGisMapPoProviderBase<ListeDerInspektionsroutenParameter, ListeDerInspektionsroutenMapParameter, ListeDerInspektionsroutenPo>, IListeDerInspektionsroutenPoProvider, IListeDerInspektionsroutenMapPoProvider
    {
        public ListeDerInspektionsroutenMapPoProvider(IListeDerInspektionsroutenMapProvider mapProviderBase)
            : base(mapProviderBase)
        {
        }

        protected override PaperType PaperType
        {
            get
            {
                return PaperType.A3Landscape;
            }
        }
    }
}
