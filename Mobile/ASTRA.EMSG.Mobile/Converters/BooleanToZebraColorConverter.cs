using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ASTRA.EMSG.Mobile.Converters
{
    public class BooleanToZebraColorConverter : IValueConverter
    {
        public Color DefaultColor { get; set; }
        public Color ZebraColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isZebraColored = (bool)value;



            return isZebraColored ? new SolidColorBrush(ZebraColor) : new SolidColorBrush(DefaultColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}