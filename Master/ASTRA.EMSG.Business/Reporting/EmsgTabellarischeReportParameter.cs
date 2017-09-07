using System.Collections.Generic;

namespace ASTRA.EMSG.Business.Reporting
{
    public class EmsgTabellarischeReportParameter : EmsgReportParameter
    {
        public List<GridFilterDescriptor> GridFilterDescriptors { get; set; }
    }
}