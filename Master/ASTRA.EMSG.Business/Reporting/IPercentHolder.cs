namespace ASTRA.EMSG.Business.Reporting
{
    public interface IPercentHolder
    {
        decimal? DecimalValue { get; set; }
        int SortOrder { get; }
        string Group { get; }
    }
}