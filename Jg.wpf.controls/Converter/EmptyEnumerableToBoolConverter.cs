using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Jg.wpf.controls.Converter
{
    public class EmptyEnumerableToBoolConverter : IValueConverter
    {
        public bool EmptyValue { get; set; }

        public EmptyEnumerableToBoolConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable enumerable)
            {
                return enumerable.GetEnumerator().MoveNext() ? !EmptyValue : EmptyValue;
            }
            else
            {
                return EmptyValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
