using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        private readonly Dictionary<Type, UserControl> _demos;

        public MainWindow()
        {
            InitializeComponent();

            _demos = new Dictionary<Type, UserControl>();

            var mainVm = new MainViewModel();
            mainVm.OnSelectDemo += OnSelectDemo;
            DataContext = mainVm;
        }

        private void OnSelectDemo(object sender, DemoItem e)
        {
            var type = Type.GetType($"Jg.wpf.app.Controls.{e.ContentType}");
            if (type != null)
            {
                var content = _demos.ContainsKey(type) ? _demos[type] : Activator.CreateInstance(type);
                if (e.DataContext != null && content is FrameworkElement element)
                {
                    element.DataContext = e.DataContext;
                }
                _demos[type] = (UserControl)content;

                MainContent.Content = content;
            }
        }


        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //MenuToggleButton.IsChecked = false;
        }

        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            DemoItemsSearchBox.Focus();
        }


    }
}
