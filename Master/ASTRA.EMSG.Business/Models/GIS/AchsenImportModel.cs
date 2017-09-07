using System;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.GIS
{
    public class AchsenImportModel : Model
    {
        public DateTime ErfassungsPeriodeDateTime { get; set; }
        public DateTime ImportDateTime { get; set; }
        public string ImportStatistic { get; set; }
        public string AchsenVersion { get; set; }
    }
}
