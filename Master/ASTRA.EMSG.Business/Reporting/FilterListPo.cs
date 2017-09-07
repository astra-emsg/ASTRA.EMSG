namespace ASTRA.EMSG.Business.Reporting
{
    public class FilterListPo
    {
        public FilterListPo(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}