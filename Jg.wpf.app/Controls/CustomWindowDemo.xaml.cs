using System.Windows;
using System.Windows.Controls;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// CustomWindowDemo.xaml 的交互逻辑
    /// </summary>
    public partial class CustomWindowDemo : UserControl
    {
        public CustomWindowDemo()
        {
            InitializeComponent();
        }

        private void ShowWindowClick(object sender, RoutedEventArgs e)
        {
            var window = new MyCustomWindow();
            window.Show();
        }

        private void ShowWindowClick2(object sender, RoutedEventArgs e)
        {
            var window = new MyCustomWindow2();
            window.Show();
        }
    }
}
