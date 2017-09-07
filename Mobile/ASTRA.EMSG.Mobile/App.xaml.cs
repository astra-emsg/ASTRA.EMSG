using System.Windows;
using System.Windows.Threading;
using ASTRA.EMSG.Common.Mobile.Logging;
using ASTRA.EMSG.Mobile.Container;
using ASTRA.EMSG.Mobile.Services;
using Autofac;

namespace ASTRA.EMSG.Mobile
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Loggers.PerformanceLogger.Debug("Application started");
            
            ClientContainerLocator.Container.Resolve<ILanguageService>().ReloadLocalization();

            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            Loggers.TechLogger.ErrorException(dispatcherUnhandledExceptionEventArgs.Exception.Message, dispatcherUnhandledExceptionEventArgs.Exception);
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Loggers.PerformanceLogger.Debug("Application closed");
        }
    }
}
