using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Jg.wpf.core.Extensions.Types.RoiTypes;

namespace Jg.wpf.controls.Converter
{
    public class RoiThicknessToWindowThicknessConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static RoiThicknessToWindowThicknessConverter _converter;

        public static RoiThicknessToWindowThicknessConverter Converter => _converter ??= new RoiThicknessToWindowThicknessConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new RoiThicknessToWindowThicknessConverter();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 0)
            {
                if (values[0] is RoiThickness roiThickness)
                {
                    return new Thickness(roiThickness.Left, roiThickness.Top, roiThickness.Right, roiThickness.Bottom);
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
