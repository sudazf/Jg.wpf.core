using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Jg.wpf.core.Service.FileService.FileTypes
{
    internal class BinaryFileImp
    {
        public void Save<T>(string filePath, T file)
        {
            try
            {
                using (var fStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    BinaryFormatter binFormat = new BinaryFormatter();
                    binFormat.Serialize(fStream, file);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public T Load<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var fStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    var binFormat = new BinaryFormatter();
                    fStream.Position = 0;
                    return (T)binFormat.Deserialize(fStream);
                }

                string message = $"Can not find file: {filePath}";
                throw new Exception(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return default;
            }
        }
    }
}
