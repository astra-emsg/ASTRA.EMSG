using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Common
{
    public interface IZustandsabschnitt : IAbschnitt, IErfassungsPeriodDependentEntity, IFlaecheFahrbahnUndTrottoirHolder, IBelastungskategorieHolder
    {
        string Strassenname { get; }
        decimal Zustandsindex { get; set; }
        ZustandsErfassungsmodus Erfassungsmodus { get; set; }
        decimal Laenge { get; set; }
        DateTime Aufnahmedatum { get; set; }
        string Aufnahmeteam { get; set; }
        WetterTyp Wetter { get; set; }
        string Bemerkung { get; set; }
        ZustandsindexTyp ZustandsindexTrottoirLinks { get; set; }
        ZustandsindexTyp ZustandsindexTrottoirRechts { get; set; }
        decimal? FlaceheTrottoirLinks { get; }
        decimal? FlaceheTrottoirRechts { get; }
        Guid? MassnahmenvorschlagKatalogId { get; }
        DringlichkeitTyp Dringlichkeit { get; }
        decimal? Kosten { get; }
        MassnahmenvorschlagKatalog MassnahmenvorschlagFahrbahn { get; set; }
        MassnahmenvorschlagKatalog MassnahmenvorschlagTrottoirLinks { get; set; }
        MassnahmenvorschlagKatalog MassnahmenvorschlagTrottoirRechts { get; set; }
        DringlichkeitTyp DringlichkeitFahrbahn { get; set; }
        DringlichkeitTyp DringlichkeitTrottoirLinks { get; set; }
        DringlichkeitTyp DringlichkeitTrottoirRechts { get; set; }
        decimal? KostenMassnahmenvorschlagFahrbahn { get; set; }
        decimal? KostenMassnahmenvorschlagTrottoirLinks { get; set; }
        decimal? KostenMassnahmenvorschlagTrottoirRechts { get; set; }
        decimal KostenFahrbahn { get; }
        decimal KostenTrottoirLinks { get; }
        decimal KostenTrottoirRechts { get; }
        IList<Schadengruppe> Schadengruppen { get; set; }
        IList<Schadendetail> Schadendetails { get; set; }
    }
}