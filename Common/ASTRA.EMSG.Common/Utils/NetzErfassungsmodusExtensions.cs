using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Common.Utils
{
    public static class NetzErfassungsmodusExtensions
    {
        public static bool IsStrasseMode(this NetzErfassungsmodus netzErfassungsmodus)
        {
            return netzErfassungsmodus == NetzErfassungsmodus.Tabellarisch ||
                   netzErfassungsmodus == NetzErfassungsmodus.Gis;
        }

        public static bool IsSummarischeMode(this NetzErfassungsmodus netzErfassungsmodus)
        {
            return netzErfassungsmodus == NetzErfassungsmodus.Summarisch;
        }
    }
}