using System;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public class ZustandsabschnittdetailsTrottoirModel : Model
    {
        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string BelastungskategorieTyp { get; set; }

        public TrottoirTyp Trottoir { get; set; }

        public Guid? LinkeTrottoirMassnahmenvorschlagKatalogId { get; set; }
        public ZustandsindexTyp LinkeTrottoirZustandsindex { get; set; }
        public DringlichkeitTyp LinkeTrottoirDringlichkeit { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.LongDecimalFormat, NullDisplayText = "")]
        public decimal? LinkeTrottoirKosten { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.LongDecimalFormat, NullDisplayText = "")]
        public decimal? LinkeTrottoirGesamtKosten { get; set; }

        public Guid? RechteTrottoirMassnahmenvorschlagKatalogId { get; set; }
        public ZustandsindexTyp RechteTrottoirZustandsindex { get; set; }
        public DringlichkeitTyp RechteTrottoirDringlichkeit { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.LongDecimalFormat, NullDisplayText = "")]
        public decimal? RechteTrottoirKosten { get; set; }
        [DisplayFormat(DataFormatString = FormatStrings.LongDecimalFormat, NullDisplayText = "")]
        public decimal? RechteTrottoirGesamtKosten { get; set; }

        public bool IsLocked { get; set; }
    }
}