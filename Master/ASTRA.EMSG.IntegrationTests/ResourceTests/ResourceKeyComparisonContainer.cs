using System.Collections.Generic;

namespace ASTRA.EMSG.IntegrationTests.ResourceTests
{
    public class ResourceKeyComparisonContainer
    {
        public List<string> MissingKeys;
        public List<string> UnnecesaryKeys;

        public ResourceKeyComparisonContainer()
        {
            MissingKeys = new List<string>();
            UnnecesaryKeys = new List<string>();
        }

        public bool NoDifferences()
        {
            return MissingKeys.Count == 0 && UnnecesaryKeys.Count == 0;
        }
    }
}