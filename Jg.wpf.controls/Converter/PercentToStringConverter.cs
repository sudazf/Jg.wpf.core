using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Jg.wpf.controls.Converter
{
    public class PercentToStringConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static PercentToStringConverter _converter;

        public static PercentToStringConverter Converter => _converter ??= new PercentToStringConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new PercentToStringConverter();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 0)
            {
                var percent = values[0] + "%";
                return percent;
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
