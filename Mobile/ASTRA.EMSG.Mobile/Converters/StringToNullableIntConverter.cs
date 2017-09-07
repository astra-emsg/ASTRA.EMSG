using System;
using System.Globalization;
using System.Windows.Data;

namespace ASTRA.EMSG.Mobile.Converters
{
    public class StringToNullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;

            if (string.IsNullOrEmpty(str))
                return null;

            return int.Parse(str);
        }
    }
}