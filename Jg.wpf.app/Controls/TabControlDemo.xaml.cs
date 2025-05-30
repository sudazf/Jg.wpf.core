﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Jg.wpf.app.ViewModels;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// TabControlDemo.xaml 的交互逻辑
    /// </summary>
    public partial class TabControlDemo : UserControl
    {
        public TabControlDemo()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is TabControlViewModel vm)
            {
                vm.Init();
            }
        }

        private void OnParentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!e.OriginalSource.Equals(sender))
            {
                return;
            }
        }
    }
}
