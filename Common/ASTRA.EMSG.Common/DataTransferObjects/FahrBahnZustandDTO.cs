using System;
using System.Collections.Generic;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.DataTransferObjects
{
    public class FahrBahnZustandDTO
    {
        public FahrBahnZustandDTO()
        {
            Schadendetails = new List<SchadendetailDTO>();
            Schadengruppen = new List<SchadengruppeDTO>();
        }
        public Guid ZustandsAbschnitt { get; set; }

        public decimal Zustandsindex { get; set; }

        public ZustandsErfassungsmodus Erfassungsmodus { get; set; }

        public IList<SchadendetailDTO> Schadendetails { get; set; }
        public IList<SchadengruppeDTO> Schadengruppen { get; set; }
    }
}