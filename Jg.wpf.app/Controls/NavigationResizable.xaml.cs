using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// NavigationResizable.xaml 的交互逻辑
    /// </summary>
    public partial class NavigationResizable : UserControl
    {
        public NavigationResizable()
        {
            InitializeComponent();

            Menu.OnNavigatedTo += Menu_OnNavigatedTo;
        }

        private void Menu_OnNavigatedTo(object sender, string e)
        {
            Menu.Content = new TextBlock { Text = e, Margin=new Thickness(16), FontSize = 25, Foreground = Brushes.LightGray,FontStyle = FontStyles.Italic};
        }
    }
}
