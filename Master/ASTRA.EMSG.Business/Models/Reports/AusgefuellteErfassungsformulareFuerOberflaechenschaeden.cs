using System;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Reports
{
    [Serializable]
    public class AusgefuellteErfassungsformulareFuerOberflaechenschaeden : Model
    {
        public string Strassenname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public string InspektionsroutenName { get; set; }
        public DateTime? Aufnahmedatum { get; set; }
        public decimal? Zustandsindex { get; set; }

        public bool IstDetaillierteSchadenserfassungsformular { get; set; }
    }
}
