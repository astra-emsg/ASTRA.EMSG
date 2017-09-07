using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.StrassenabschnitteListe
{
    public interface IStrassenabschnitteListeMapPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W1_2)]
    [ReportInfo(AuswertungTyp.W1_6, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class StrassenabschnitteListeMapPoProvider : EmsgGisMapPoProviderBase<StrassenabschnitteListeParameter, StrassenabschnitteListeMapParameter, StrassenabschnitteListePo>, IStrassenabschnitteListeMapPoProvider
    {
        public StrassenabschnitteListeMapPoProvider(IStrassenabschnitteListeMapProvider mapProviderBase) : base(mapProviderBase)
        {
        }
    }
}