using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Strassennamen
{
    [Serializable]
    public class ExportStrassenabschnittModel
    {
        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string ExternalId { get; set; }
        public int? Abschnittsnummer { get; set; }
        public EigentuemerTyp Strasseneigentuemer { get; set; }
        public string Ortsbezeichnung { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public BelagsTyp Belag { get; set; }
        public decimal? BreiteFahrbahn { get; set; }
        public decimal? Laenge { get; set; }
        public TrottoirTyp Trottoir { get; set; }
        public decimal? BreiteTrottoirLinks { get; set; }
        public decimal? BreiteTrottoirRechts { get; set; }
    }
}
