using System;
using ASTRA.EMSG.Business.Infrastructure.Reporting;
using Kendo.Mvc;

namespace ASTRA.EMSG.Web.Areas.Auswertungen.ReportGridCommands
{
    public class ReportGridCommand : GridCommand
    {
        public OutputFormat? OutputFormat { get; set; }
        public Guid? ErfassungsPeriodId { get; set; }
    }
}