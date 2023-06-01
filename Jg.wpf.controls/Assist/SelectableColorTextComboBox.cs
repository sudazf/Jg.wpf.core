using System;
using System.Windows.Controls;
using System.Windows;

namespace Jg.wpf.controls.Assist
{
    public class SelectableColorTextComboBox : ComboBox
    {
        public static readonly RoutedEvent DisplayChangedEvent = EventManager.RegisterRoutedEvent("DisplayChanged", RoutingStrategy.Bubble, typeof(EventHandler<DisplayChangedEventArgs>), typeof(SelectableColorTextComboBox));

        public event RoutedEventHandler DisplayChanged
        {
            add => AddHandler(DisplayChangedEvent, value);
            remove => RemoveHandler(DisplayChangedEvent, value);
        }

        public string Display
        {
            get => (string)GetValue(DisplayProperty);
            set => SetValue(DisplayProperty, value);
        }

        public static readonly DependencyProperty DisplayProperty =
            DependencyProperty.Register("Display", typeof(string), typeof(SelectableColorTextComboBox), new PropertyMetadata("", OnDisplayChanged));

        public void SelectDisplay()
        {
            if (GetTemplateChild("PART_DisplayTextBox") is TextBox displayTextBox)
            {
                displayTextBox.Focus();
                displayTextBox.SelectAll();
            }
        }

        private static void OnDisplayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SelectableColorTextComboBox comboBox)
            {
                comboBox.RaiseEvent(new DisplayChangedEventArgs(DisplayChangedEvent, comboBox));
            }
        }
    }

    public class DisplayChangedEventArgs : RoutedEventArgs
    {
        public DisplayChangedEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
        }
    }
}
