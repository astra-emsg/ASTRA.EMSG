using System.ComponentModel;
using ASTRA.EMSG.Mobile.Utils;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged
    {
        bool IsVisible { get; set; }
    }

    public class ViewModel : NotifyPropertyChanged, IViewModel
    {
        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; Notify(() => IsVisible); }
        }

        public ViewModel()
        {
            IsVisible = true;
        }
    }
}