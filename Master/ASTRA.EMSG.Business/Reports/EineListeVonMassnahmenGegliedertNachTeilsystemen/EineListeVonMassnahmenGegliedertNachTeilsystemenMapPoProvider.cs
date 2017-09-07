using System.Linq;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.EineListeVonMassnahmenGegliedertNachTeilsystemen
{
    public interface IEineListeVonMassnahmenGegliedertNachTeilsystemenMapPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W4_1)]
    [ReportInfo(AuswertungTyp.W4_2, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class EineListeVonMassnahmenGegliedertNachTeilsystemenMapPoProvider : EmsgGisMapPoProviderBase<EineListeVonMassnahmenGegliedertNachTeilsystemenParameter, EineListeVonMassnahmenGegliedertNachTeilsystemenMapParameter, EineListeVonMassnahmenGegliedertNachTeilsystemenPo>, IEineListeVonMassnahmenGegliedertNachTeilsystemenPoProvider, IEineListeVonMassnahmenGegliedertNachTeilsystemenMapPoProvider
    {
        public EineListeVonMassnahmenGegliedertNachTeilsystemenMapPoProvider(IEineListeVonMassnahmenGegliedertNachTeilsystemenMapProvider mapProviderBase)
            : base(mapProviderBase)
        {
        }
    }
}