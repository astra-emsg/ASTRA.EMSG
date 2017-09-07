using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Mobile.ViewModels;
using ASTRA.EMSG.Mobile.Events;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface IProgressService
    {
        event EventHandler<ProgressStartEventArgs> OnStart;
        event EventHandler<EventArgs> OnStop;
        event EventHandler<ProgressUpdateEventArgs> OnUpdate;
        void Start(string text);
        void Update(string text, float percent);
        void Stop();
    }
    public class ProgressService : IProgressService
    {

        public event EventHandler<ProgressStartEventArgs> OnStart;
        public event EventHandler<EventArgs> OnStop;
        public event EventHandler<ProgressUpdateEventArgs> OnUpdate;
        
        public void Start(string text)
        {
            if (this.OnStart != null)
            {
                this.OnStart(this, new ProgressStartEventArgs(text));
            }
        }
        public void Update(string text, float percent)
        {
            if (this.OnUpdate != null)
            {
                this.OnUpdate(this, new ProgressUpdateEventArgs(text, percent));
            }
        }
        public void Stop()
        {
            if (this.OnStop != null)
            {
                this.OnStop(this, EventArgs.Empty);
            }
        }
    }
}
