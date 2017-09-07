using ASTRA.EMSG.Business.Entities.Mapping;

namespace ASTRA.EMSG.Business.Entities.Common
{
    [TableShortName("BGC")]
    public class BenchmarkingGruppenConfiguration : Entity
    {
        public virtual string EigenschaftTyp { get; set; }
        public virtual decimal Grenzwert { get; set; }
    }
}