using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Models.Common
{
    public class BenchmarkingDataModel : Model
    {
        public decimal GesamtlaengeDesStrassennetzesProSiedlungsflaeche { get; set; }
        public decimal GesamtlaengeDesStrassennetzesProEinwohner { get; set; }

        public decimal FahrbahnflaecheProSiedlungsflaeche { get; set; }
        public decimal FahrbahnflaecheProEinwohner { get; set; }

        public decimal GesamtstrassenflaecheProSiedlungsflaeche { get; set; }
        public decimal GesamtstrassenflaecheProEinwohner { get; set; }

        public decimal WiederbeschaffungswertProFahrbahn { get; set; }
        public decimal WiederbeschaffungswertProEinwohner { get; set; }

        public decimal WertverlustProFahrbahn { get; set; }
        public decimal WertverlustProEinwohner { get; set; }
    }
}
