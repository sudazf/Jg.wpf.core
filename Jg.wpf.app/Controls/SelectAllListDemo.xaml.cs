using Jg.wpf.controls.Customer.SelectAll;
using System.Windows;
using System.Windows.Controls;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// SelectAllListDemo.xaml 的交互逻辑
    /// </summary>
    public partial class SelectAllListDemo : UserControl
    {
        public SelectAllListDemo()
        {
            InitializeComponent();
        }

        private void SelectAllListBox_OnSelectAllChanged(object sender, RoutedEventArgs e)
        {
            if (e is SelectAllEventArgs arg)
            {
                MessageBox.Show(arg.IsSelectAll ? "已手动全选" : "已手动取消全选", "系统提示", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}
