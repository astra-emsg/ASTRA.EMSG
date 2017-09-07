using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace ASTRA.EMSG.Mobile.Installer.Utils
{
    public class LanguageSelector : INotifyPropertyChanged
    {
        private GermanLabels localizedLabels = new GermanLabels();
        public GermanLabels LocalizedLabels { get { return localizedLabels; } }

        public void SetLanguage(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            switch (cultureInfo.TwoLetterISOLanguageName.ToUpper())
            {
                case "FR":
                    localizedLabels = new FrenchLabels();
                    break;
                case "IT":
                    localizedLabels = new ItalianLabels();
                    break;
                default:
                    localizedLabels = new GermanLabels();
                    break;
            }
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("LocalizedLabels"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}