using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ASTRA.EMSG.Business.Reporting
{
    [Serializable]
    public class ReportSizeCollection
    {
        public List<ReportSize> ReportSizes { get; set; }
    }

    [Serializable]
    public class ReportSize
    {
        [XmlAttribute]
        public int ColumnCount { get; set; }

        [XmlAttribute]
        public decimal ChartLeft { get; set; }

        [XmlAttribute]
        public decimal ColumnWidthCorrection { get; set; }

    }
}