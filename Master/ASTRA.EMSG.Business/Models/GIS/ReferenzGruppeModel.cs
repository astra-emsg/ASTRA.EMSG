using System;
using ASTRA.EMSG.Common;
using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class ReferenzGruppeModel : Model
    {
        public ReferenzGruppeModel()
        {
            
            AchsenReferenzenModel = new List<AchsenReferenzModel>();
            
        }

        public List<AchsenReferenzModel> AchsenReferenzenModel;
        public StrassenabschnittGISModel StrassenabschnittGISModel { get; set; } 
        public ZustandsabschnittGISModel ZustandsabschnittGISModel { get; set; }

        public void AddAchsenReferenz(AchsenReferenzModel achsenReferenzModel)
        {
            achsenReferenzModel.ReferenzGruppeModel = this;
            AchsenReferenzenModel.Add(achsenReferenzModel);
            
        }
        
    }
}