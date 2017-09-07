using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;


namespace ASTRA.EMSG.Mobile.Model
{
    public class LayerSelectorItem
    {
        public string Id { get; set; }
        public string Displayname { get; set; }
        public ICommand LegendCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        //public ICommand UncheckCommand { get; set; }
        public bool HasLegend { get { return LegendCommand != null; } }
        public bool IsChecked { get; set; }
        public override string ToString()
        {
            return Displayname;
        }
    }
}
