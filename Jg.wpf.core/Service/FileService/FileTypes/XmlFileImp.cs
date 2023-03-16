using System;
using System.IO;
using System.Xml.Serialization;

namespace Jg.wpf.core.Service.FileService.FileTypes
{
    internal class XmlFileImp
    {
        public void Save<T>(string fullPath, T obj)
        {
            try
            {
                using (var fStream = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    var xmlFormat = new XmlSerializer(typeof(T));
                    xmlFormat.Serialize(fStream, obj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

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
                Console.WriteLine(e);
                return default;
            }
        }
    }
}
