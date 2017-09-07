namespace ASTRA.EMSG.Tests.ResourceTests
{
    public class ResourceValueComparisonContainer
    {
        public string DefaultValue { get; set; }
        public string ComparedValue { get; set; }

        public ResourceValueComparisonContainer(string defaultValue, string comparedValue)
        {
            DefaultValue = defaultValue;
            ComparedValue = comparedValue;
        }
    }
}