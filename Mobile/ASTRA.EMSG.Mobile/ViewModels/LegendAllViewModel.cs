using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTRA.EMSG.Common.Utils;
using ASTRA.EMSG.Localization;

namespace ASTRA.EMSG.Mobile.ViewModels
{
    public class LegendAllViewModel : ViewModel
    {
        public IList<LegendViewModel> LegendViewModelList { get; set; }

        public LegendAllViewModel(IList<LegendViewModel> legendViewModelList)
        {
            LegendViewModelList = legendViewModelList;
        }
    }
}
