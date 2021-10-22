using System.Windows;
using Jg.wpf.core.Service;

namespace Jg.wpf.app
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            BaseService.Register(Current.Dispatcher);
        }
    }
}
