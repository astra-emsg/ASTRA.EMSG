using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Kendo.Mvc;
using Kendo.Mvc.UI;

namespace ASTRA.EMSG.Web.Infrastructure.TelerikExtensions
{
    public class SortValueProvider : IValueProvider
    {
        private readonly IList<SortDescriptor> sortDescriptors;

        public SortValueProvider(IList<SortDescriptor> sortDescriptors)
        {
            this.sortDescriptors = sortDescriptors;
        }

        public bool ContainsPrefix(string prefix)
        {
            return false;
        }

        public ValueProviderResult GetValue(string key)
        {
            if (key == GridUrlParameters.Sort)
            {
                var par = GridDescriptorSerializer.Serialize(sortDescriptors);
                return new ValueProviderResult(par, par, CultureInfo.InvariantCulture);
            }
            return null;
        }
    }
}