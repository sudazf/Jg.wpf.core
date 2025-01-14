using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Jg.wpf.app.ViewModels;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// MultiSelectComboBoxDemo.xaml 的交互逻辑
    /// </summary>
    public partial class MultiSelectComboBoxDemo : UserControl
    {
        public MultiSelectComboBoxDemo()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is MultiSelectComboBoxViewModel vm)
            {
                var selects = vm.SelectContainer.Items.Where(a => a.IsSelected).Select(a => a.Name);
                var message = $"You select: {string.Join(",", selects)}";

                Task.Factory.StartNew(() => Thread.Sleep(10)).ContinueWith(t =>
                {
                    MySnackbar.MessageQueue?.Enqueue(message);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}
