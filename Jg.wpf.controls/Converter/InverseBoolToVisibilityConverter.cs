using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

namespace Jg.wpf.controls.Converter
{
    public class InverseBoolToVisibilityConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static InverseBoolToVisibilityConverter _converter;

        public static InverseBoolToVisibilityConverter Converter => _converter ??= new InverseBoolToVisibilityConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new InverseBoolToVisibilityConverter();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0 && bool.TryParse(values[0].ToString(), out bool visible))
            {
                return visible ? Visibility.Collapsed : Visibility.Visible;
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
