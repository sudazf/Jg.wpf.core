using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Jg.wpf.core.Log;
using Jg.wpf.core.Service.FileService;
using Jg.wpf.core.Service.Resource;

namespace Jg.wpf.core.Service
{
    public static class ServiceRegistration
    {
        public static void Regist()
        {
            ResourceManager.Initialize(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "etc"));
            InitializeLogger();

            ServiceManager.RegisterService("FileDialogService", new FileDialogServiceImp());
            ServiceManager.RegisterService("XmlFileService", new XmlFileServiceImp());
            ServiceManager.RegisterService("TxtFileService", new TxtFileServiceImp());
        }

        private static void InitializeLogger()
        {
            Logger.LogLevel = TraceLevel.Info;
            DateTime currentTime = DateTime.Now;
            string newFileName = $"{currentTime:yyyyMMdd_HHmmss}.log";

            var logDirectory = ResourceManager.GetValue("LogSettings", "Directory", @"d:\SolarLogs");

            if (!Directory.Exists(logDirectory))
            {
                var desireDisk = Path.GetPathRoot(logDirectory);
                if (string.IsNullOrEmpty(desireDisk))
                {
                    logDirectory = @"C:\SolarLogs";
                }
                else
                {
                    var disks = (IList)Directory.GetLogicalDrives();
                    if (!disks.Contains(desireDisk.ToUpper()))
                    {
                        logDirectory = @"C:\SolarLogs";
                    }
                }

                Directory.CreateDirectory(logDirectory);
            }

            var fullName = Path.Combine(logDirectory, newFileName);
            var listener = new TextWriterTraceListener(fullName, "myListener");
            Trace.Listeners.Add(listener);
            Logger.FileName = fullName;
        }
    }
}
