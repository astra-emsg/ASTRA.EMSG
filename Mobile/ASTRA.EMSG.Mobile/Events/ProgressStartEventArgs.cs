using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTRA.EMSG.Mobile.Events
{
    public class ProgressStartEventArgs : EventArgs
    {
        public string ProgressText { get; set; }

        public ProgressStartEventArgs(string progresstext)
        {
            this.ProgressText = progresstext;
        }
    }
}
