using System.ComponentModel;

namespace ASTRA.EMSG.Localization
{
    public class LocalizationLocator : INotifyPropertyChanged
    {
        public static readonly LocalizationLocator Instance;
        static LocalizationLocator()
        {
            Instance = new LocalizationLocator();
        }
        private LocalizationLocator() { }

        private MobileLocalization mobileLocalization = new MobileLocalization();
        public static void SetMobileLocalization(MobileLocalization l)
        {
            Instance.mobileLocalization = l;
            Instance.OnPropertyChanged("MobileLocalization");
        }

        public static MobileLocalization MobileLocalization { get { return Instance.mobileLocalization; } }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}