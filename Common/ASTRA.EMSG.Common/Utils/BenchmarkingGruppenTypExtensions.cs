using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.Utils
{
    public static class BenchmarkingGruppenTypExtensions
    {
        public static string ToDataBaseEigenschaftTyp(this BenchmarkingGruppenTyp benchmarkingGruppenTyp)
        {
            return benchmarkingGruppenTyp.ToString() + "Grenze";
        }
    }
}