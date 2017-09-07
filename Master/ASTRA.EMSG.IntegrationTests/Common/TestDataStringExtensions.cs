namespace ASTRA.EMSG.IntegrationTests.Common
{
    public static class TestDataStringExtensions
    {
        public static string GetStrassennameBezeichnungVon(this string strassenname)
        {
            return string.Format("{0}Von", strassenname);
        }

        public static string GetStrassennameBezeichnungBis(this string strassenname)
        {
            return string.Format("{0}Bis", strassenname);
        }
        
        public static string GetZustandsabschnittBezeichnungVon(this string strassennameVon)
        {
            return string.Format("{0}ZA_Von", strassennameVon);
        }
        
        public static string GetZustandsabschnittBezeichnungBis(this string strassennameBis)
        {
            return string.Format("{0}ZA_Bis", strassennameBis);
        }

        public static string GetInInspektionBei(this string inspektionsroutename)
        {
            return string.Format("{0}InInspektionBei", inspektionsroutename);
        }
    }
}