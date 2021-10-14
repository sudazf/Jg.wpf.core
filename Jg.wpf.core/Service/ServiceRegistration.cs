using System;
using System.IO;
using Jg.wpf.core.Log;
using Jg.wpf.core.Service.FileService;
using Jg.wpf.core.Service.Resource;
using Jg.wpf.core.Utility;

namespace Jg.wpf.core.Service
{
    public static class ServiceRegistration
    {
        public static void Regist()
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

            ServiceManager.RegisterService("FileDialogService", new FileDialogServiceImp());
            ServiceManager.RegisterService("XmlFileService", new XmlFileServiceImp());
            ServiceManager.RegisterService("TxtFileService", new TxtFileServiceImp());
        }
    }
}
