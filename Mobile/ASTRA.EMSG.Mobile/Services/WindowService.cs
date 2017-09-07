using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using ASTRA.EMSG.Common.Mobile.Logging;
using ASTRA.EMSG.Localization;
using ASTRA.EMSG.Mobile.ViewModels;
using ASTRA.EMSG.Mobile.Views.Windows;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface IWindowService
    {
        void OpenHelpWindow(string helpFileName);
        //void OpenLegendWindowAll();
        void OpenZustandFahrbahnWindow(ZustandFahrbahnWindowViewModel zustandFahrbahnWindowViewModel, Action action);
        void OpenLegendWindow(LegendViewModel legendViewModel);
        void CloseAllLegendWindows();
        //void CloseLegendWindowAll();
    }

    public class WindowService : IWindowService
    {
        private readonly IClientConfigurationProvider clientConfigurationProvider;
        private readonly IMessageBoxService messageBoxService;
        private IList<LegendWindow> currentOpenLegendWindows;
        private HelpWindow currentOpenHelpWindow;

        public WindowService(IClientConfigurationProvider clientConfigurationProvider, IMessageBoxService messageBoxService)
        {
            this.clientConfigurationProvider = clientConfigurationProvider;
            this.messageBoxService = messageBoxService;            
            currentOpenLegendWindows = new List<LegendWindow>();            
        }
        
        public void OpenZustandFahrbahnWindow(ZustandFahrbahnWindowViewModel zustandFahrbahnWindowViewModel, Action action)
        {
            var zustandFahrbahnWindow = new ZustandFahrbahnWindow { DataContext = zustandFahrbahnWindowViewModel };
            zustandFahrbahnWindowViewModel.Closed += (sender, args) => zustandFahrbahnWindow.Close();
            zustandFahrbahnWindowViewModel.Saved += (sender, args) => zustandFahrbahnWindow.Close();
            zustandFahrbahnWindow.Closed += (sender, args) => action();
            zustandFahrbahnWindow.Closing += (sender, args) => { zustandFahrbahnWindowViewModel.Closing(); };

            zustandFahrbahnWindow.ShowDialog();
            zustandFahrbahnWindow.Focus();
        }

        public void OpenLegendWindow(LegendViewModel legendViewModel)
        {
            var openWindow = currentOpenLegendWindows.Where(lw => ((LegendViewModel)lw.DataContext).LegendName == legendViewModel.LegendName);
            if (openWindow.Count() == 0)
            {

                var legendWindow = new LegendWindow { DataContext = legendViewModel };
                currentOpenLegendWindows.Add(legendWindow);
                legendWindow.Closed += LegendWindowClosed;
                legendWindow.Show();
            }
            else
            {
                openWindow.Single().Focus();
            }
        }

        void LegendWindowClosed(object sender, EventArgs e)
        {
            currentOpenLegendWindows.Remove((LegendWindow)sender);
        }     

        public void OpenHelpWindow(string helpFileName)
        {
            if (currentOpenHelpWindow == null)
            {
                currentOpenHelpWindow = new HelpWindow();
                currentOpenHelpWindow.Closed += (sender, args) => currentOpenHelpWindow = null;
                currentOpenHelpWindow.Show();
            }

            currentOpenHelpWindow.Focus();

            string applicationDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            string helpFile = GetHelpFilePath(helpFileName, clientConfigurationProvider.PackageFolderPath); 
   
            if (!File.Exists(helpFile))  
                helpFile = GetHelpFilePath(helpFileName, applicationDirectory);

            if (!File.Exists(helpFile))
            {
                messageBoxService.Warning(LocalizationLocator.MobileLocalization.HelpNotExistsWarning);
                Loggers.TechLogger.Warn(string.Format("Help file not exists {0}", helpFile));
                return;
            }

            currentOpenHelpWindow.help.Navigate(helpFile);
        }

        private string GetHelpFilePath(string helpFileName, string applicationDirectory)
        {
            return Path.Combine(applicationDirectory, string.Format("{0}\\{1}\\{2}", clientConfigurationProvider.HelpDirectoryPath, Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower(), helpFileName));
        }

        public void CloseAllLegendWindows()
        {
            foreach (var lw in currentOpenLegendWindows)
            {
                lw.Closed -= LegendWindowClosed;
                lw.Close();
            }
            currentOpenLegendWindows.Clear();
        }
    }
}
