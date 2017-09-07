using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("BDD")]
    public class BenchmarkingDataDetail : Entity
    {
        public virtual Belastungskategorie Belastungskategorie { get; set; }
        
        //Unit: %
        public virtual decimal FahrbahnflaecheAnteil { get; set; }

        //Unit: 0.0m - 5.0m
        public virtual decimal? Zustandsindex { get; set; }
        
        //Unit: %
        public virtual decimal RealisierteMassnahmenProWiederbeschaffungswertNetz { get; set; }

        public virtual BenchmarkingData BenchmarkingData { get; set; }
    }
}