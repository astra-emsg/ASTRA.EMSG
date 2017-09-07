using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Business.Entities;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.GIS;
using ASTRA.EMSG.Business.Entities.Strassennamen;
using ASTRA.EMSG.Business.Infrastructure.Filtering;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Business.Reporting;
using ASTRA.EMSG.Common.Enums;
using ASTRA.EMSG.Common.Utils;
using NHibernate.Linq;

namespace ASTRA.EMSG.Business.Reports.MassnahmenvorschlagProZustandsabschnitt
{
    public interface IMassnahmenvorschlagProZustandsabschnittMapPoProvider : IPoProvider
    {
    }

    [ReportInfo(AuswertungTyp.W3_8, NetzErfassungsmodus = NetzErfassungsmodus.Gis)]
    public class MassnahmenvorschlagProZustandsabschnittMapPoProvider : EmsgGisMapPoProviderBase<MassnahmenvorschlagProZustandsabschnittParameter, MassnahmenvorschlagProZustandsabschnittMapParameter, MassnahmenvorschlagProZustandsabschnittPo>, IMassnahmenvorschlagProZustandsabschnittPoProvider, IMassnahmenvorschlagProZustandsabschnittMapPoProvider
    {
        public MassnahmenvorschlagProZustandsabschnittMapPoProvider(IMassnahmenvorschlagProZustandsabschnittMapProvider mapProviderBase)
            : base(mapProviderBase)
        {
        }
    }
}