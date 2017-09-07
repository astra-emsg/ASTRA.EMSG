using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Common
{
    public interface IStrassenabschnitt : IAbschnitt, IErfassungsPeriodDependentEntity, IFlaecheFahrbahnUndTrottoirHolder, IBelastungskategorieHolder
    {
        string Strassenname { get; set; }
        string BelastungskategorieTyp { get; }
        BelagsTyp Belag { get; set; }
        decimal Laenge { get; set; }
        decimal BreiteFahrbahn { get; set; }
        TrottoirTyp Trottoir { get; set; }
        decimal? BreiteTrottoirLinks { get; set; }
        decimal? BreiteTrottoirRechts { get; set; }
        EigentuemerTyp Strasseneigentuemer { get; set; }
        string Ortsbezeichnung { get; set; }
        decimal GesamtFlaeche { get; }
    }
}
