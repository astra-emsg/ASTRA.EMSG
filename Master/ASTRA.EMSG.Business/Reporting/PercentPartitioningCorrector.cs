using System;
using System.Collections.Generic;
using System.Linq;

namespace ASTRA.EMSG.Business.Reporting
{
    public static class PercentPartitioningCorrector
    {
        public static void Corrigate(List<IPercentHolder> percentHolders)
        {
            var groupList = percentHolders.GroupBy(ph => ph.Group).ToList();
            foreach (var g in groupList)
            {
                decimal roundigError = 0;
                var sortedList = g.OrderBy(i => i.SortOrder).ToList();
                foreach (var item in sortedList)
                {
                    if(item.DecimalValue.HasValue && item.DecimalValue.Value > 0)
                    {
                        var decimalToRound = item.DecimalValue.Value + roundigError;
                        var roundedValue = Math.Round(decimalToRound, 0);
                        roundigError = decimalToRound - roundedValue;
                        item.DecimalValue = roundedValue;
                    }
                }
            }
        }
    }
}
