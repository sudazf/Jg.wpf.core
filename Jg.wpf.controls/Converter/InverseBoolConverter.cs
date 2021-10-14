using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Jg.wpf.controls.Converter
{
    public class InverseBoolConverterExtensions : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static InverseBoolConverterExtensions _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new InverseBoolConverterExtensions());
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0 && bool.TryParse((values[0] ?? "").ToString(), out bool res))
            {
                return !res;
            }
            return false;
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
