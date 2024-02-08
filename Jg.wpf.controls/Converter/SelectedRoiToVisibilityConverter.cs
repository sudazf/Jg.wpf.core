using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Jg.wpf.core.Extensions.Types.RoiTypes;
using Jg.wpf.core.Utility;

namespace Jg.wpf.controls.Converter
{
    public class SelectedRoiToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static SelectedRoiToVisibilityConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var value = _converter;
            if (value != null)
            {
                return value;
            }

            return (_converter = new SelectedRoiToVisibilityConverter());
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Roi roi)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
