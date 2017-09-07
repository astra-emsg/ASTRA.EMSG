using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.ZustandProZustandsabschnitt
{
    public interface IZustandProZustandsabschnittMapPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W3_7, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class ZustandProZustandsabschnittMapPoProvider : EmsgGisMapPoProviderBase<ZustandProZustandsabschnittParameter, ZustandProZustandsabschnittMapParameter, ZustandProZustandsabschnittPo>, IZustandProZustandsabschnittPoProvider, IZustandProZustandsabschnittMapPoProvider
    {
        public ZustandProZustandsabschnittMapPoProvider(IZustandProZustandsabschnittMapProvider mapProviderBase)
            : base(mapProviderBase)
        {
        }
    }
}