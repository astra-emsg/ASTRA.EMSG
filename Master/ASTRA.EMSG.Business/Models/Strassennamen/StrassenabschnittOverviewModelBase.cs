using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    [Serializable]
    public class StrassenabschnittOverviewModelBase : Model
    {
        public string Strassenname { get; set; }
        public int? Abschnittsnummer { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string Ortsbezeichnung { get; set; }
        public string ExternalId { get; set; }
        public Guid Belastungskategorie { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }
        public decimal Laenge { get; set; }
        public decimal? FlaecheFahrbahn { get; set; }
        public decimal? FlaecheTrottoir { get; set; }
    }
}