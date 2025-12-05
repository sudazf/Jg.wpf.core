using System.Windows;
using Jg.wpf.controls.Customer.CustomWindows;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// MyCustomWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MyCustomWindow : CustomWindow
    {
        public MyCustomWindow()
        {
            InitializeComponent();
        }

        private void OnTestClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("hello~", "god:", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
