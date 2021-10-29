using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace Jg.wpf.controls.Customer.Navigation
{
    public class NavigatorItem : Control
    {
        public event EventHandler<string> OnNavigated;
        public NavigatorItem()
        {
            MouseDown += OnMouseDown;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            OnNavigated?.Invoke(this, Display);
            IsSelected = true;
        }

        public PackIconKind Icon    
        {
            get => (PackIconKind)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(PackIconKind), typeof(NavigatorItem), new PropertyMetadata(PackIconKind.CursorDefault));

        public string Display
        {
            get => (string)GetValue(DisplayProperty);
            set => SetValue(DisplayProperty, value);
        }

        public static readonly DependencyProperty DisplayProperty =
            DependencyProperty.Register("Display", typeof(string), typeof(NavigatorItem), new PropertyMetadata("Unset Name"));


        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(NavigatorItem), new PropertyMetadata(false));

    }
}
