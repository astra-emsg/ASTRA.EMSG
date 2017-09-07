using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reports.EineListeVonMassnahmenGegliedertNachTeilsystemen
{
    [Serializable]
    public class EineListeVonMassnahmenGegliedertNachTeilsystemenPo
    {
        public Guid Id { get; set; }

        public string Projektname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }

        public decimal Laenge { get; set; } 

        public TeilsystemTyp Teilsystem { get; set; }
        public string TeilsystemBezeichnung { get; set; }
        
        public string ZustaendigeOrganisation { get; set; }

        public DringlichkeitTyp Dringlichkeit { get; set; }
        public string DringlichkeitBezeichnung { get; set; }

        public decimal? Kosten { get; set; }
        public string Beschreibung { get; set; }

        public StatusTyp Status { get; set; }
        public string StatusBezeichnung { get; set; }
    }
}
