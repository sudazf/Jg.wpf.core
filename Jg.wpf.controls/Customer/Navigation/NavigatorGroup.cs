using System.Windows;
using System.Windows.Controls.Primitives;

namespace Jg.wpf.controls.Customer.Navigation
{
    public class NavigatorGroup : Selector
    {
        public string GroupName
        {
            get => (string)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }

        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(NavigatorGroup), new PropertyMetadata("Unset Name"));

    }
}
