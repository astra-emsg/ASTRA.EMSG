using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Reporting
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ReportInfoAttribute : Attribute
    {
        public AuswertungTyp ReportType { get; set; }

        public NetzErfassungsmodus NetzErfassungsmodus { get; set; }

        public ReportInfoAttribute(AuswertungTyp reportType)
        {
            NetzErfassungsmodus = (NetzErfassungsmodus) (-1);
            this.ReportType = reportType;
        }
    }
}
