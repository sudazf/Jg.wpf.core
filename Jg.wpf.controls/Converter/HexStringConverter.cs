using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Jg.wpf.controls.Converter
{
    public class HexStringConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private string _lastValidValue;
        private static HexStringConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new HexStringConverter());
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string ret = null;

            if (values?.Length > 0 && values[0] != null && values[0] is string)
            {
                var valueAsString = (string)values[0];
                var parts = valueAsString.ToCharArray();
                var formatted = parts.Select((p, i) => (++i) % 2 == 0 ? string.Concat(p.ToString(), " ") : p.ToString());
                ret = string.Join(string.Empty, formatted).Trim();
            }
            return ret;
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
            object ret = null;
            if (value != null && value is string)
            {
                var valueAsString = ((string)value).Replace(" ", String.Empty).ToUpper();
                ret = _lastValidValue = IsHex(valueAsString) ? valueAsString : _lastValidValue;
            }

            return ret;
        }

        private bool IsHex(string text)
        {
            var reg = new System.Text.RegularExpressions.Regex("^[0-9A-Fa-f]*$");
            return reg.IsMatch(text);
        }

    }
}
