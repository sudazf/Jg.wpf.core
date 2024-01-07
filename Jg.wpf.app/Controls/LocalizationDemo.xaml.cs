using System.Windows;
using System.Windows.Controls;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// LocalizationDemo.xaml 的交互逻辑
    /// </summary>
    public partial class LocalizationDemo : UserControl
    {
        public LocalizationDemo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //to test in dialog is for checking whether this markup extension has memory leak problem.
            var localizationWindowDemo = new LocalizationWindowDemo
            {
                Owner = Application.Current.MainWindow,
                DataContext = this.DataContext
            };

            localizationWindowDemo.ShowDialog();
        }
    }
}
