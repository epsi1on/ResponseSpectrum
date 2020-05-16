using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SeismicResponseSpectrum.Ui
{
    public class ParametricVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!object.ReferenceEquals(value, null) && !object.ReferenceEquals(parameter, null))
                return value.ToString() == parameter.ToString() ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
