using System;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Models.GIS
{
    [Serializable]
    public class KoordinierteMassnahmeGISOverviewModel : Model
    {
        public string Projektname { get; set; }
        public string BezeichnungVon { get; set; }
        public string BezeichnungBis { get; set; }
        public DateTime? AusfuehrungsAnfang { get; set; }
        public DateTime? AusfuehrungsEnde { get; set; }
        public StatusTyp Status { get; set; }
        public string StatusBezeichnung { get; set; }
        public decimal? KostenGesamtprojekt { get; set; }
        public string LeitendeOrganisation { get; set; }
    }
}