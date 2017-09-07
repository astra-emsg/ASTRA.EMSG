using ASTRA.EMSG.Common.Enums;

namespace ASTRA.EMSG.Business.Entities.Common
{
    public interface IFlaecheProvider
    {
        TrottoirTyp Trottoir { get; }
        decimal Laenge { get; }
        decimal BreiteFahrbahn { get; }
        decimal? BreiteTrottoirLinks { get; }
        decimal? BreiteTrottoirRechts { get; }
    }

    public static class FlaecheProviderExtension
    {
        public static bool HasTrottoirInformation(this IFlaecheProvider flaecheProvider)
        {
            return flaecheProvider.Trottoir != TrottoirTyp.NochNichtErfasst;
        }

        public static bool HasTrottoir(this IFlaecheProvider flaecheProvider)
        {
            return flaecheProvider.Trottoir != TrottoirTyp.NochNichtErfasst && flaecheProvider.Trottoir != TrottoirTyp.KeinTrottoir;
        } 

        public static decimal GesamtFlaeche(this IFlaecheProvider flaecheProvider)
        {
            return flaecheProvider.Laenge*(flaecheProvider.BreiteFahrbahn + (flaecheProvider.BreiteTrottoirLinks ?? 0) + (flaecheProvider.BreiteTrottoirRechts ?? 0));
        }

        public static decimal FlaecheTrottoir(this IFlaecheProvider flaecheProvider)
        {
            return flaecheProvider.FlaecheTrottoirLinks() + flaecheProvider.FlaecheTrottoirRechts();
        }

        public static decimal FlaecheFahrbahn(this IFlaecheProvider flaecheProvider)
        {
            return flaecheProvider.Laenge * flaecheProvider.BreiteFahrbahn;
        }
 
        public static decimal FlaecheTrottoirLinks(this IFlaecheProvider flaecheProvider)
        {
            return flaecheProvider.Laenge * flaecheProvider.BreiteTrottoirLinks ?? 0;
        } 

        public static decimal FlaecheTrottoirRechts(this IFlaecheProvider flaecheProvider)
        {
            return flaecheProvider.Laenge * flaecheProvider.BreiteTrottoirRechts ?? 0;
        } 


    }
}