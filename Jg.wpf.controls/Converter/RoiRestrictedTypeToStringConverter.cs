using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using Jg.wpf.core.Utility;

namespace Jg.wpf.controls.Converter
{
    public class RoiRestrictedTypeToStringConverter : MarkupExtension, IValueConverter
    {
        private static RoiRestrictedTypeToStringConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var value = _converter;
            if (value != null)
            {
                return value;
            }

            return (_converter = new RoiRestrictedTypeToStringConverter());
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) 
                return DependencyProperty.UnsetValue;

            var description = GetDescription((Enum)value);

            return TranslateHelper.Translate(description);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private string GetDescription(Enum en)
        {
            var type = en.GetType();
            var memInfo = type.GetMember(en.ToString());
            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }
    }
}
