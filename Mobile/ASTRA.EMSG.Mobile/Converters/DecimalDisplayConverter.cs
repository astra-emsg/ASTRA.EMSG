using System;
using System.Globalization;
using System.Windows.Data;

namespace ASTRA.EMSG.Mobile.Converters
{
    public class DecimalDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            return ((decimal)value).ToString("0.0");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}