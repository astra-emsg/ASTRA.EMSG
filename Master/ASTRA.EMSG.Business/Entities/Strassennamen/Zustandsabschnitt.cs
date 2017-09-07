using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Strassennamen
{
    [TableShortName("ZST")]
    public class Zustandsabschnitt : ZustandsabschnittBase
    {
        public override string Strassenname { get { return Strassenabschnitt.Strassenname; } }
        public override StrassenabschnittBase StrassenabschnittBase { get { return Strassenabschnitt; } }

        public virtual string StrassennameBezeichnungVon { get { return Strassenabschnitt.BezeichnungVon; } }
        public virtual string StrassennameBezeichnungBis { get { return Strassenabschnitt.BezeichnungBis; } }

        public override string InspektionsroutenName { get { return string.Empty; } }

        public virtual Strassenabschnitt Strassenabschnitt { get; set; }
    }
}
