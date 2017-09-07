using ASTRA.EMSG.Business.Reporting;

namespace ASTRA.EMSG.Tests.PercentCorrigatorTests
{
    public class Percent : IPercentHolder
    {
        public Percent(decimal? decimalValue)
        {
            DecimalValue = decimalValue;
        }

        public Percent(decimal? decimalValue, int sortOrder)
        {
            SortOrder = sortOrder;
            DecimalValue = decimalValue;
        }

        public Percent(decimal? decimalValue, int sortOrder, string groupName)
        {
            DecimalValue = decimalValue;
            SortOrder = sortOrder;
            Group = groupName;
        }

        public decimal? DecimalValue { get; set; }
        public int SortOrder { get; private set; }
        public string Group { get; private set; }
    }
}