using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class EineListeVonMassnahmenGegliedertNachTeilsystemenGridCommand : GisReportGridCommand
    {
        public string Projektname { get; set; }

        public int? Status { get; set; }
        public StatusTyp? StatusTyp { get { return Status.HasValue ? (StatusTyp)Status.Value : (StatusTyp?)null; } }

        public int? Dringlichkeit { get; set; }
        public DringlichkeitTyp? DringlichkeitTyp { get { return Dringlichkeit.HasValue ? (DringlichkeitTyp)Dringlichkeit.Value : (DringlichkeitTyp?)null; } }

        public int? Teilsystem { get; set; }
        public TeilsystemTyp? TeilsystemTyp { get { return Teilsystem.HasValue ? (TeilsystemTyp)Teilsystem.Value : (TeilsystemTyp?)null; } }
    }
}