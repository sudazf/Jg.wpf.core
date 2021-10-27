using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Jg.wpf.app.Models;
using Jg.wpf.app.ViewModels;

namespace Jg.wpf.app
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var mainVm = new MainViewModel();
            mainVm.OnSelectDemo += OnSelectDemo;
            DataContext = mainVm;
        }

        private void OnSelectDemo(object sender, DemoItem e)
        {
            var type = Type.GetType($"Jg.wpf.app.Controls.{e.ContentType}");
            if (type != null)
            {
                var content = Activator.CreateInstance(type);
                if (e.DataContext != null && content is FrameworkElement element)
                {
                    element.DataContext = e.DataContext;
                }

                MainContent.Content = content;
            }
        }


        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
        }

        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            DemoItemsSearchBox.Focus();
        }


    }
}
