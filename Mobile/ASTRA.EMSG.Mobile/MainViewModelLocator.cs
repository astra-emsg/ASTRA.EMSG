using ASTRA.EMSG.Mobile.Container;
using ASTRA.EMSG.Mobile.ViewModels;
using Autofac;

namespace ASTRA.EMSG.Mobile
{
    public class MainViewModelLocator
    {
        private readonly IMainWindowViewModel mainWindowViewModel = ClientContainerLocator.Container.Resolve<IMainWindowViewModel>();
        public IMainWindowViewModel MainWindowViewModel { get { return mainWindowViewModel; } }
    }
}
