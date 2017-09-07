using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Mobile.Events
{
    public class ProgressUpdateEventArgs: EventArgs
    {
       public string ProgressText { get; set; }
        public float Progress { get; set; }
        public ProgressUpdateEventArgs(string progresstext, float progress)
        {
            this.Progress = progress;
            this.ProgressText = progresstext;
        }
    }
}
