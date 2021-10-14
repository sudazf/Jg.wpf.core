using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Jg.wpf.controls.Converter
{
    public class BoolToVisibilityConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static BoolToVisibilityConverter _converter;

        public static BoolToVisibilityConverter Converter => _converter ?? (_converter = new BoolToVisibilityConverter());

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new BoolToVisibilityConverter());
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0 && bool.TryParse(values[0].ToString(), out bool visible))
            {
                return visible ? Visibility.Visible : Visibility.Collapsed;
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
