using System;
using System.IO;
using System.Windows.Threading;
using Jg.wpf.core.Log;
using Jg.wpf.core.Service.FileService;
using Jg.wpf.core.Service.Resource;
using Jg.wpf.core.Service.ThemeService;
using Jg.wpf.core.Service.ThreadService;
using Jg.wpf.core.Utility;

namespace Jg.wpf.core.Service
{
    internal static class BaseService
    {
        /// <summary>
        /// 注册基础 Service
        /// </summary>
        /// <param name="dispatcher">当前程序 Dispatcher</param>
        public static void Register(Dispatcher dispatcher)
        {
            var etc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "etc");
            var language = Path.Combine(etc, "Language");

            if (!Directory.Exists(etc))
            {
                Directory.CreateDirectory(etc);
            }

            ResourceManager.Initialize(etc);
            Logger.Initialize();
            TranslateHelper.Initialize(language);

            ServiceManager.RegisterService("DispatcherService", new DispatcherServiceImp(dispatcher));
            ServiceManager.RegisterService("FileDialogService", new FileDialogServiceImp());
            ServiceManager.RegisterService("XmlFileService", new XmlFileServiceImp());
            ServiceManager.RegisterService("TxtFileService", new TxtFileServiceImp());
            ServiceManager.RegisterService("ThemeService", new ThemeServiceImp());
        }
    }
}
