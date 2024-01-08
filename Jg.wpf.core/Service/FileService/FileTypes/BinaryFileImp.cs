using Microsoft.Win32;
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
                    var binFormat = new BinaryFormatter();
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
                    using (var fStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        var binFormat = new BinaryFormatter();
                        fStream.Position = 0;
#pragma warning disable SYSLIB0011
                        return (T)binFormat.Deserialize(fStream);
#pragma warning restore SYSLIB0011
                    }
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

        public void SaveAs<T>(string folderPath, T fileObject)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Save-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".binary";
            if (!string.IsNullOrEmpty(folderPath))
            {
                if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
                saveFileDialog.InitialDirectory = folderPath;
            }

            var res = saveFileDialog.ShowDialog();
            if (res != null && res.Value)
            {
                Save(saveFileDialog.FileName, fileObject);
            }
        }
    }
}
