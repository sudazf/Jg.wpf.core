using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Jg.wpf.app.ViewModels;

namespace Jg.wpf.app.LocalConverters
{
    public class SelectContainerToItemsConverter : MarkupExtension, IValueConverter
    {
        private static SelectContainerToItemsConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new SelectContainerToItemsConverter());
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MyContainer container)
            {
                return container.Items;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
