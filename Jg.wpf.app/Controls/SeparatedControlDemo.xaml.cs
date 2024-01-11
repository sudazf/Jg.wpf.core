using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Jg.wpf.core.Service.Performance;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// SeparatedControlDemo.xaml 的交互逻辑
    /// </summary>
    public partial class SeparatedControlDemo : UserControl
    {
        private readonly UiPerformanceGuard _uiPerformanceGuard;

        public SeparatedControlDemo()
        {
            InitializeComponent();

            _uiPerformanceGuard = new UiPerformanceGuard(3000);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _uiPerformanceGuard.Start("My work");

            Task.Run(() => 
            {
                Thread.Sleep(5000);
                //...
                _uiPerformanceGuard.Stop();
            });
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(AfterWindowLoaded), DispatcherPriority.Background);
        }

        private void AfterWindowLoaded()
        {
            _uiPerformanceGuard.Init(Indicator.ThreadDispatcher, Indicator.UiContent);
        }
    }
}
