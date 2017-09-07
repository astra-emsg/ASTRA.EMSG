using System;
using System.Collections.Generic;
using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("BDT")]
    public class BenchmarkingData : Entity, IErfassungsPeriodDependentEntity
    {
        public BenchmarkingData()
        {
            BenchmarkingDataDetails = new List<BenchmarkingDataDetail>();
        }
        
        //Unit: km/ha
        public virtual decimal GesamtlaengeDesStrassennetzesProSiedlungsflaeche { get; set; }
        //Unit: m/E
        public virtual decimal GesamtlaengeDesStrassennetzesProEinwohner { get; set; }

        //Unit: m2/ha
        public virtual decimal FahrbahnflaecheProSiedlungsflaeche { get; set; }
        //Unit: m2/E
        public virtual decimal FahrbahnflaecheProEinwohner { get; set; }

        //Unit: %
        public virtual decimal GesamtstrassenflaecheProSiedlungsflaeche { get; set; }
        //Unit: m2/E
        public virtual decimal GesamtstrassenflaecheProEinwohner { get; set; }

        //Unit: CHF/m2
        public virtual decimal WiederbeschaffungswertProFahrbahn { get; set; }
        //Unit: CHF/E
        public virtual decimal WiederbeschaffungswertProEinwohner { get; set; }

        //Unit: CHF/m2
        public virtual decimal WertverlustProFahrbahn { get; set; }
        //Unit: CHF/E
        public virtual decimal WertverlustProEinwohner { get; set; }

        //Unit: 0.0m - 5.0m
        public virtual decimal? ZustandsindexNetz { get; set; }

        //Unit: DateTime
        public virtual DateTime? MittleresAlterDerZustandsaufnahmenNetz { get; set; }

        //Unit: CHF/m2
        public virtual decimal RealisierteMassnahmenProFahrbahn { get; set; }
        
        //Unit: CHF/E
        public virtual decimal RealisierteMassnahmenProEinwohner { get; set; }
        
        //Unit: %
        public virtual decimal RealisierteMassnahmenProWertverlustNetz { get; set; }
        
        //Unit: %
        public virtual decimal RealisierteMassnahmenProWiederbeschaffungswertNetz { get; set; }

        public virtual IList<BenchmarkingDataDetail> BenchmarkingDataDetails { get; set; }

        public virtual ErfassungsPeriod ErfassungsPeriod { get; set; }
        
        public virtual Mandant Mandant { get; set; }

        public virtual DateTime CalculatedAt { get; set; }

        public virtual bool NeedsRecalculation { get; set; }
    }
}