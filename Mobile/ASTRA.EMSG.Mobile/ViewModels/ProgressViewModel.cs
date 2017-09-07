using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Mobile.Services;
using ASTRA.EMSG.Mobile.Events;
using System.ComponentModel;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public interface IProgressViewModel : IViewModel, INotifyPropertyChanged
    {
        float ProgressValue { get; set; }
        string ProgressText { get; set; }
    }


    public class ProgressViewModel : ViewModel, IFormViewModel, IProgressViewModel
    {
        private float progressValue;
        private string progressText;
        public float ProgressValue { get { return progressValue; } set { progressValue = value; Notify(()=>ProgressValue); } }
        public string ProgressText { get { return progressText; } set { progressText = value; Notify(() => ProgressText); } }
        public ProgressViewModel(IProgressService progressService)
        {
            progressService.OnStart += progressServiceOnStart;
            progressService.OnUpdate += progressServiceOnUpdate;
            progressService.OnStop += progressServiceOnStop;
            this.IsVisible = false;
        }

        void progressServiceOnStart(object sender, ProgressStartEventArgs e)
        {
            this.IsVisible = true;
            this.ProgressText = e.ProgressText;
            this.ProgressValue = 0;
        }
        void progressServiceOnUpdate(object sender, ProgressUpdateEventArgs e)
        {
            this.ProgressValue = e.Progress;
            this.ProgressText = e.ProgressText;
        }
        void progressServiceOnStop(object sender, EventArgs e)
        {
            this.IsVisible = false;
            this.ProgressValue = 0;
            this.ProgressText = String.Empty;            
        }
    }
}
