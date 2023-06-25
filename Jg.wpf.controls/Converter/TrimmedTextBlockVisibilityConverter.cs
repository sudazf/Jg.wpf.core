using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows;

namespace Jg.wpf.controls.Converter
{
    public class TrimmedTextBlockVisibilityConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static TrimmedTextBlockVisibilityConverter _converter;
        public static TrimmedTextBlockVisibilityConverter Converter => _converter ??= new TrimmedTextBlockVisibilityConverter();
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new TrimmedTextBlockVisibilityConverter();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0)
            {
                if (values[0] == null) { return Visibility.Collapsed; }

                if (values[0] is TextBlock textBlock)
                {
                    Typeface typeface = new Typeface(
                        textBlock.FontFamily,
                        textBlock.FontStyle,
                        textBlock.FontWeight,
                        textBlock.FontStretch);

                    FormattedText formattedText = new FormattedText(textBlock.Text,
                        System.Threading.Thread.CurrentThread.CurrentCulture, textBlock.FlowDirection, typeface,
                        textBlock.FontSize,
                        textBlock.Foreground, VisualTreeHelper.GetDpi(textBlock).PixelsPerDip);

                    textBlock.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

                    if (textBlock.ActualWidth < formattedText.Width)
                    { return Visibility.Visible; }
                }
            }

            return Visibility.Collapsed;
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
