using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Jg.wpf.core.Extensions.Types;

namespace Jg.wpf.controls.Converter
{
    public class JColorToBrushConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static JColorToBrushConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new JColorToBrushConverter();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0)
            {
                if (values[0] is JColor color)
                {
                    return new SolidColorBrush(Color.FromRgb((byte)color.R, (byte)color.G, (byte)color.B));
                }
            }

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(new[] { value }, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var convertedColor = ColorConverter.ConvertFromString(value.ToString());
                if (convertedColor is Color color)
                {
                    var jColor = new JColor(color.R, color.G, color.B);
                    return jColor;
                }
            }

            return Binding.DoNothing;
        }

    }
}
