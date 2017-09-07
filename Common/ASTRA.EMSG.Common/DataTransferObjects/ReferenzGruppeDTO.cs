using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    [Serializable]
    public class ReferenzGruppeDTO:DataTransferObject
    {
        public ReferenzGruppeDTO()
        {
            AchsenReferenzenDTO = new List<AchsenReferenzDTO>();
        }
        public List<AchsenReferenzDTO> AchsenReferenzenDTO { get; set; } 
        public StrassenabschnittGISDTO StrassenabschnittGISDTO { get; set; } 
        public ZustandsabschnittGISDTO ZustandsabschnittGISDTO { get; set; }
        public void AddAchsenReferenz(AchsenReferenzDTO achsenReferenzDTO)
        {
            achsenReferenzDTO.ReferenzGruppeDTO = this;
            AchsenReferenzenDTO.Add(achsenReferenzDTO);
        }
    }
}
