using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Jg.wpf.controls.Converter
{
    public class StringFormatConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static StringFormatConverter _converter;

        public static StringFormatConverter Converter => _converter ??= new StringFormatConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Converter;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 )
            {
                var value = (double)values[0];
                var decimals = (int)values[1];
                return Math.Round(value, decimals).ToString(CultureInfo.InvariantCulture);
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (double.TryParse(value.ToString(), out var result))
                {
                    return new object[] { result };
                }
            }

            return null;
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
