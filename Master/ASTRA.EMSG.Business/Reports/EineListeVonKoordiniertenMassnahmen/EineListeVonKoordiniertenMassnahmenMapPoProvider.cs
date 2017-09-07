using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.EineListeVonKoordiniertenMassnahmen
{
    public interface IEineListeVonKoordiniertenMassnahmenMapPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W4_3)]
    [ReportInfo(AuswertungTyp.W4_4, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class EineListeVonKoordiniertenMassnahmenMapPoProvider : EmsgGisMapPoProviderBase<EineListeVonKoordiniertenMassnahmenParameter, EineListeVonKoordiniertenMassnahmenMapParameter, EineListeVonKoordiniertenMassnahmenPo>, IEineListeVonKoordiniertenMassnahmenPoProvider, IEineListeVonKoordiniertenMassnahmenMapPoProvider
    {
        public EineListeVonKoordiniertenMassnahmenMapPoProvider(IEineListeVonKoordiniertenMassnahmenMapProvider mapProviderBase)
            : base(mapProviderBase)
        {
        }
    }
}
