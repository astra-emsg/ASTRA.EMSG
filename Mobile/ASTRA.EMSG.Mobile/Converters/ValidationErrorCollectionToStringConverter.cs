using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ASTRA.EMSG.Mobile.Converters
{
    public class ValidationErrorCollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var readOnlyObservableCollection = (ReadOnlyObservableCollection<ValidationError>)value;
            return string.Join(Environment.NewLine, readOnlyObservableCollection.Select(e => e.ErrorContent));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}