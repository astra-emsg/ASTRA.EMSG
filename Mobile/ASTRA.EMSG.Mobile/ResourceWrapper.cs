using ASTRA.EMSG.Localization;
using ASTRA.EMSG.Mobile.Utils;

namespace ASTRA.EMSG.Mobile
{
    public sealed class ResourceWrapper : NotifyPropertyChanged
    {
        public MobileLocalization MobileLocalization { get { return LocalizationLocator.MobileLocalization; } }

        public void Refresh()
        {
            Notify(() => MobileLocalization);
        }
    }
}
