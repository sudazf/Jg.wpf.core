using System;
using System.Collections.ObjectModel;
using System.Windows;
using Jg.wpf.core.Service;
using Jg.wpf.core.Service.ThemeService;

namespace Jg.wpf.app
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceManager.Init(Current.Dispatcher);

            var themes = ServiceManager.GetService<IThemeService>();
            themes.Apply(true);
        }
    }
}
