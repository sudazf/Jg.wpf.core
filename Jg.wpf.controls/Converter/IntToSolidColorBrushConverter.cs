using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Jg.wpf.controls.Converter
{
    public class IntToSolidColorBrushConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static IntToSolidColorBrushConverter _converter;

        public static IntToSolidColorBrushConverter Converter => _converter ??= new IntToSolidColorBrushConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new IntToSolidColorBrushConverter();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 0)
            {
                int colorIndex = (int)values[0];
                SolidColorBrush brush;
                switch (colorIndex % 5)
                {
                    case 0:
                        brush = Brushes.DeepSkyBlue;
                        break;
                    case 1:
                        brush = Brushes.Orange;
                        break;
                    case 2:
                        brush = Brushes.Green;
                        break;
                    case 3:
                        brush = Brushes.Brown;
                        break;
                    case 4:
                        brush = Brushes.Pink;
                        break;
                    default:
                        brush = Brushes.SlateBlue;
                        break;
                }
                return brush;
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
            throw new NotImplementedException();
        }

    }
}
