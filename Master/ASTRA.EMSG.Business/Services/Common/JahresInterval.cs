namespace ASTRA.EMSG.Business.Services.Common
{
    public class JahresInterval
    {
        private readonly int von;
        private readonly int bis;

        public JahresInterval(int von, int bis)
        {
            this.von = von;
            this.bis = bis;
        }

        public int JahrBis
        {
            get { return bis; }
        }

        public int JahrVon
        {
            get { return von; }
        }
    }
}