using System.Collections.Generic;
using System.Linq;

namespace ASTRA.EMSG.Common.Utils
{
    public static class IEnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

    }
}
