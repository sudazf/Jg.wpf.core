using System;
using System.IO;
using System.Xml.Serialization;
using Jg.wpf.core.Log;

namespace Jg.wpf.core.Service.FileService
{
    internal class XmlFileServiceImp : IXmlFileService
    {
        public T Load<T>(string fullPath)
        {
            try
            {
                if (File.Exists(fullPath))
                {
                    using (Stream fStream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                        fStream.Position = 0;
                        return (T)xmlFormat.Deserialize(fStream);
                    }
                }

                string message = $"未找到文件 {fullPath}！";
                throw new Exception(message);
            }
            catch (Exception e)
            {
                Logger.WriteLineError(e.Message);
                throw;
            }
        }

        public T Load<T>(string fullPath, Type[] types)
        {
            try
            {
                if (File.Exists(fullPath))
                {
                    using (Stream fStream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        XmlSerializer xmlFormat = new XmlSerializer(typeof(T), types);
                        fStream.Position = 0;
                        return (T)xmlFormat.Deserialize(fStream);
                    }
                }

                string message = $"未找到文件 {fullPath}！";
                throw new Exception(message);
            }
            catch (Exception e)
            {
                Logger.WriteLineError(e.Message);
                throw;
            }
        }

        public void Save<T>(T obj, string fullPath)
        {
            try
            {
                using (Stream fStream = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                    xmlFormat.Serialize(fStream, obj);
                }
            }
            catch (Exception e)
            {
                Logger.WriteLineError(e.Message);
                throw;
            }
        }

        public void Save<T>(T obj, string fullPath, Type[] types)
        {
            try
            {
                using (Stream fStream = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(T), types);
                    xmlFormat.Serialize(fStream, obj);
                }
            }
            catch (Exception e)
            {
                Logger.WriteLineError(e.Message);
                throw;
            }
        }
    }
}
