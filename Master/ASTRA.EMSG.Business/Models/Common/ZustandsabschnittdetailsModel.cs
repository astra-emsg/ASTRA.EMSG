using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASTRA.EMSG.Common;
using System.Linq;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public class ZustandsabschnittdetailsModel : Model
    {
        public ZustandsabschnittdetailsModel()
        {
            SchadengruppeModelList = new List<SchadengruppeModel>();
        }

        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string BelastungskategorieTyp { get; set; }
        public BelagsTyp Belag { get; set; }

        public decimal? Zustandsindex { get; set; }
        public decimal ZustandsindexCalculated { get { return !SchadengruppeModelList.Any() ? (Zustandsindex ?? 0) : Math.Min(decimal.Divide(1, 10)*SchadengruppeModelList.Sum(sgm => sgm.Bewertung), 5); } }

        public Guid? MassnahmenvorschlagKatalog { get; set; }
        public DringlichkeitTyp Dringlichkeit { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.LongDecimalFormat, NullDisplayText = "")]
        public decimal KostenFahrbahn { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = FormatStrings.LongDecimalFormat, NullDisplayText = "")]
        public decimal? Kosten { get; set; }

        public ZustandsErfassungsmodus Erfassungsmodus { get; set; }

        public int Schadensumme { get { return SchadengruppeModelList.Sum(sgm => sgm.Bewertung); } }

        public List<SchadengruppeModel> SchadengruppeModelList { get; set; }

        public bool IsLocked { get; set; }

        public bool IsGrobInitializiert { get; set; }
        public bool IsDetailInitializiert { get; set; }
    }
}