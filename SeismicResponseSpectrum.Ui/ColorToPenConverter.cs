using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace SeismicResponseSpectrum.Ui
{
    /*
    internal class ColorToPenConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color)
            {
                return new Pen(new SolidColorBrush((Color) value), ApplicationSettings.Current.ChartPenWidth);
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    internal class PenMultyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Brush))
            {
                return new SolidColorBrush((values.First(i => i is SeismicRecordSpectrum) as SeismicRecordSpectrum).Color);
            };

            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public enum PenProperty
        {
            PenWidth,
            PenColor
        }
    }.
    */
}
