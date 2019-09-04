using System;
using System.Globalization;
using Xamarin.Forms;

namespace GradientControl.Converters
{
    public class SKColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SkiaSharp.SKColor.Parse(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SkiaSharp.SKColor)value).ToString();
        }
    }
}
