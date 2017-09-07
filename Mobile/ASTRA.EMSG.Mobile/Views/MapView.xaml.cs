using System.Windows.Controls;
using ASTRA.EMSG.Mobile.ViewModels;
using System.Windows.Controls.Primitives;

namespace ASTRA.EMSG.Mobile.Views
{
    /// <summary>
    /// Interaction logic for KarteView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();
        }
        
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((IMapViewModel)this.DataContext).UserControl_Loaded(sender, e);
        }
       
        private void InspektionsRoutenSwitch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext != null)
                ((IMapViewModel)this.DataContext).InspektionsRoutenSwitch_SelectionChanged(sender , e);
        }
    }
}
