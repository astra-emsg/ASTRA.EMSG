using System;
using System.Collections.Specialized;

namespace ASTRA.EMSG.Business.Infrastructure.Reporting
{
    public interface IReportParameter
    {
        Microsoft.Reporting.WebForms.ReportParameter ReportParameter { get; }
    }

    public class ServerReportParameter : IReportParameter
    {
        public Microsoft.Reporting.WebForms.ReportParameter ReportParameter { get; private set; }

        public ServerReportParameter(string name, string value)
        {
            ReportParameter = new Microsoft.Reporting.WebForms.ReportParameter(name, value);
        }
    }

    public class LazyServerReportParameter : IReportParameter
    {
        private readonly Lazy<Microsoft.Reporting.WebForms.ReportParameter> reportParameter;

        public LazyServerReportParameter(string name, Func<object> valueProvider)
        {
            reportParameter = new Lazy<Microsoft.Reporting.WebForms.ReportParameter>(
                () =>
                {
                    var value = valueProvider();
                    return new Microsoft.Reporting.WebForms.ReportParameter(name, value == null ? string.Empty : value.ToString());
                });
        }

        public Microsoft.Reporting.WebForms.ReportParameter ReportParameter
        {
            get { return reportParameter.Value; }
        }
    }
}
