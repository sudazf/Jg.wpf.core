using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Jg.wpf.controls.Customer.CustomWindows
{
    public class CustomWindow : Window
    {
        public int WindowTitleHeight
        {
            get => (int)GetValue(WindowTitleHeightProperty);
            set => SetValue(WindowTitleHeightProperty, value);
        }

        public static readonly DependencyProperty WindowTitleHeightProperty =
            DependencyProperty.Register(nameof(WindowTitleHeight), typeof(int), typeof(CustomWindow), new PropertyMetadata(36));

        public static readonly DependencyProperty CloseButtonHoverBackgroundProperty = DependencyProperty.Register(
            nameof(CloseButtonHoverBackground), typeof(Brush), typeof(CustomWindow),
            new PropertyMetadata(default(Brush)));

        public Brush CloseButtonHoverBackground
        {
            get => (Brush)GetValue(CloseButtonHoverBackgroundProperty);
            set => SetValue(CloseButtonHoverBackgroundProperty, value);
        }

        public static readonly DependencyProperty CloseButtonHoverForegroundProperty =
            DependencyProperty.Register(
                nameof(CloseButtonHoverForeground), typeof(Brush), typeof(CustomWindow),
                new PropertyMetadata(default(Brush)));

        public Brush CloseButtonHoverForeground
        {
            get => (Brush)GetValue(CloseButtonHoverForegroundProperty);
            set => SetValue(CloseButtonHoverForegroundProperty, value);
        }

        public static readonly DependencyProperty CloseButtonBackgroundProperty = DependencyProperty.Register(
            nameof(CloseButtonBackground), typeof(Brush), typeof(CustomWindow), new PropertyMetadata(Brushes.Transparent));

        public Brush CloseButtonBackground
        {
            get => (Brush)GetValue(CloseButtonBackgroundProperty);
            set => SetValue(CloseButtonBackgroundProperty, value);
        }

        public static readonly DependencyProperty CloseButtonForegroundProperty = DependencyProperty.Register(
            nameof(CloseButtonForeground), typeof(Brush), typeof(CustomWindow),
            new PropertyMetadata(Brushes.White));

        public Brush CloseButtonForeground
        {
            get => (Brush)GetValue(CloseButtonForegroundProperty);
            set => SetValue(CloseButtonForegroundProperty, value);
        }

        public static readonly DependencyProperty OtherButtonBackgroundProperty = DependencyProperty.Register(
            nameof(OtherButtonBackground), typeof(Brush), typeof(CustomWindow), new PropertyMetadata(Brushes.Transparent));

        public Brush OtherButtonBackground
        {
            get => (Brush)GetValue(OtherButtonBackgroundProperty);
            set => SetValue(OtherButtonBackgroundProperty, value);
        }

        public static readonly DependencyProperty OtherButtonForegroundProperty = DependencyProperty.Register(
            nameof(OtherButtonForeground), typeof(Brush), typeof(CustomWindow),
            new PropertyMetadata(Brushes.White));

        public Brush OtherButtonForeground
        {
            get => (Brush)GetValue(OtherButtonForegroundProperty);
            set => SetValue(OtherButtonForegroundProperty, value);
        }

        public static readonly DependencyProperty OtherButtonHoverBackgroundProperty = DependencyProperty.Register(
            nameof(OtherButtonHoverBackground), typeof(Brush), typeof(CustomWindow),
            new PropertyMetadata(default(Brush)));

        public Brush OtherButtonHoverBackground
        {
            get => (Brush)GetValue(OtherButtonHoverBackgroundProperty);
            set => SetValue(OtherButtonHoverBackgroundProperty, value);
        }

        public static readonly DependencyProperty OtherButtonHoverForegroundProperty =
            DependencyProperty.Register(
                nameof(OtherButtonHoverForeground), typeof(Brush), typeof(CustomWindow),
                new PropertyMetadata(default(Brush)));

        public Brush OtherButtonHoverForeground
        {
            get => (Brush)GetValue(OtherButtonHoverForegroundProperty);
            set => SetValue(OtherButtonHoverForegroundProperty, value);
        }

        public static readonly DependencyProperty NonClientAreaContentProperty = DependencyProperty.Register(
            nameof(NonClientAreaContent), typeof(object), typeof(CustomWindow), new PropertyMetadata(default(object)));

        public object NonClientAreaContent
        {
            get => GetValue(NonClientAreaContentProperty);
            set => SetValue(NonClientAreaContentProperty, value);
        }

        public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register(
            nameof(ShowIcon), typeof(bool), typeof(CustomWindow), new PropertyMetadata(true));

        public bool ShowIcon
        {
            get => (bool)GetValue(ShowIconProperty);
            set => SetValue(ShowIconProperty, value);
        }

        public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
            nameof(ShowTitle), typeof(bool), typeof(Window), new PropertyMetadata(true));

        public bool ShowTitle
        {
            get => (bool)GetValue(ShowTitleProperty);
            set => SetValue(ShowTitleProperty, value);
        }

        public CustomWindow()
        {
            DefaultStyleKey = typeof(CustomWindow);
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
        }

        public event Action CloseWindowEvent;

        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);
        //    if (e.ButtonState == MouseButtonState.Pressed)
        //        DragMove();
        //}

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (SizeToContent == SizeToContent.WidthAndHeight)
                InvalidateMeasure();
        }

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (null != CloseWindowEvent)
            {
                CloseWindowEvent();
            }
            this.Close();
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element == null)
                return;

            var point = WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
                : new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
            point = element.TransformToAncestor(this).Transform(point);
            SystemCommands.ShowSystemMenu(this, point);
        }
    }
}
