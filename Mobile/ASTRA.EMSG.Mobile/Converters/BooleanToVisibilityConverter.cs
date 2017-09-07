using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ASTRA.EMSG.Mobile.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public Visibility InvisibleState { get; set; }
        public bool IsInverted { get; set; }

        public BooleanToVisibilityConverter()
        {
            InvisibleState = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = (bool)value;

            if (IsInverted)
                visible = !visible;

            if (visible)
            {
                return Visibility.Visible;
            }

            return InvisibleState;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility)value;

            bool visible = visibility == Visibility.Visible;

            if (IsInverted)
                visible = !visible;

            return visible;
        }
    }
}
