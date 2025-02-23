using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Media;
using Brushes = System.Drawing.Brushes;

namespace Jg.wpf.controls.Converter
{

    public class BoolToPageButtonBackgroundConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static BoolToPageButtonBackgroundConverter _converter;

        public static BoolToPageButtonBackgroundConverter Converter => _converter ??= new BoolToPageButtonBackgroundConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new BoolToPageButtonBackgroundConverter();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 0 && values[0] is bool val)
            {
                if (val)
                {
                    if (Application.Current.MainWindow != null)
                        return Application.Current.MainWindow.FindResource("PrimaryHueMidBrush") as SolidColorBrush;
                }
                else
                {
                    return Brushes.Transparent;
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
            throw new NotImplementedException();
        }

    }
}
