using System.Windows;

namespace Jg.wpf.controls.Customer.SelectAll
{
    public class SelectAllEventArgs : RoutedEventArgs
    {
        public bool IsSelectAll { get; }
        public SelectAllEventArgs(RoutedEvent routedEvent, object source, bool isSelectAll) : base(routedEvent, source)
        {
            IsSelectAll = isSelectAll;
        }
    }
}
