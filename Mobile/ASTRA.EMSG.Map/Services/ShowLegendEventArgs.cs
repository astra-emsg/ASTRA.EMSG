using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Map.Services
{
    public class ShowLegendEventArgs: EventArgs
    {
        public string Layer { get; set; }
        public ShowLegendEventArgs(string layer)
        {
            this.Layer = layer;
        }
    }
}
