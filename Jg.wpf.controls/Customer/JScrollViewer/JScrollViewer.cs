using System.Windows;
using System.Windows.Controls;

namespace Jg.wpf.controls.Customer.JScrollViewer
{
    public class JScrollViewer : ScrollViewer
    {
        public static readonly DependencyProperty BindableHorizontalOffsetProperty =
            DependencyProperty.Register("BindableHorizontalOffset", typeof(double), typeof(JScrollViewer),
                new FrameworkPropertyMetadata(BindableHorizontalOffsetPropertyChanged));

        public static readonly DependencyProperty BindableVerticalOffsetProperty =
            DependencyProperty.Register("BindableVerticalOffset", typeof(double), typeof(JScrollViewer),
                new FrameworkPropertyMetadata(BindableVerticalOffsetPropertyChanged));

        public double BindableHorizontalOffset
        {
            get => (double)GetValue(BindableHorizontalOffsetProperty);
            set => SetValue(BindableHorizontalOffsetProperty, value);
        }

        public double BindableVerticalOffset
        {
            get => (double)GetValue(BindableVerticalOffsetProperty);
            set => SetValue(BindableVerticalOffsetProperty, value);
        }


        private static void BindableHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JScrollViewer obj)
            {
                obj.ScrollToHorizontalOffset((double)(e.NewValue));
            }
        }

        private static void BindableVerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JScrollViewer obj)
            {
                obj.ScrollToVerticalOffset((double)(e.NewValue));
            }
        }
    }
}
