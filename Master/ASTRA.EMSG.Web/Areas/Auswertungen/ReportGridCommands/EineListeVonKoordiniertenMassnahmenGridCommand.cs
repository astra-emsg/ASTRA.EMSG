using System;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class EineListeVonKoordiniertenMassnahmenGridCommand : GisReportGridCommand
    {
        public int? Status { get; set; }
        public StatusTyp? StatusTyp { get { return Status.HasValue ? (StatusTyp)Status.Value : (StatusTyp?)null; } }

        public string Projektname { get; set; }

        public DateTime? AusfuehrungsanfangVon { get; set; }
        public DateTime? AusfuehrungsanfangBis { get; set; }
    }
}