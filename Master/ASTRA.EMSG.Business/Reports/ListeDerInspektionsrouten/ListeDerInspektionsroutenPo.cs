using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.ListeDerInspektionsrouten
{
    [Serializable]
    public class ListeDerInspektionsroutenPo
    {
        public Guid Id { get; set; }
        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        public EigentuemerTyp Strasseneigentuemer { get; set; }
        public string StrasseneigentuemerBezeichnung { get; set; }

        public string BelastungskategorieTyp { get; set; }
        public string BelastungskategorieBezeichnung { get; set; }

        public decimal FlaecheFahrbahn { get; set; }
        public decimal FlaecheTrottoirLinks { get; set; }
        public decimal FlaecheTrottoirRechts { get; set; }

        public string Inspektionsroutename { get; set; }
        public int Reihenfolge { get; set; }
        public string InInspektionBei { get; set; }
        public DateTime? InInspektionBis { get; set; }
        public string ImageUrl { get; set; }
        public string ImageContent { get; set; }
    }
}
