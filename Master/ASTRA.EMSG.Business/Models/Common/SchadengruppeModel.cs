using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.Common
{
    [Serializable]
    public class SchadengruppeModel : Model
    {
        public SchadengruppeModel()
        {
            SchadendetailModelList = new List<SchadendetailModel>();
        }

        public Guid ZustandsabschnittId { get; set; }
        
        public SchadenschwereTyp SchadenschwereTyp { get; set; }
        public SchadenausmassTyp SchadenausmassTyp { get; set; }

        public int Matrix { get { return (int)SchadenschwereTyp * (int)SchadenausmassTyp; } }

        public int MatrixCalculated { get { return SchadendetailModelList.Any() ? SchadendetailModelList.Max(sdm => sdm.Matrix) : 0; } }

        public int Gewicht { get; set; }
        public int Bewertung { get { return (!SchadendetailModelList.Any() ? Matrix : MatrixCalculated) * Gewicht; } }

        public SchadengruppeTyp SchadengruppeTyp { get; set; }

        public List<SchadendetailModel> SchadendetailModelList { get; set; }
    }
}
