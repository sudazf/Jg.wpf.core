using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using Jg.wpf.app.ViewModels;

namespace Jg.wpf.app.LocalConverters
{
    public class SelectContainerToTextConverter : MarkupExtension, IValueConverter
    {
        private static SelectContainerToTextConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new SelectContainerToTextConverter());
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MyContainer container)
            {
                var items = container.Items.Where(s => s.IsSelected).Select(s => s.Name);
                return string.Join(",", items);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
