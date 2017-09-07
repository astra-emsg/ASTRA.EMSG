using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Localization;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class LegendViewModel : ViewModel
    {
        public string LegendName { get; set; }
        public string ImagePath { get; set; }
        public string WindowTitle { get; private set; }
        public LegendViewModel(string Path, string name)
        {
            LegendName = name;
            WindowTitle = string.Format("{0} {1}", LocalizationLocator.MobileLocalization.Legend, name);
            this.ImagePath = Path;
        }
    }
}
