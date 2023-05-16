using System;
using System.IO;
using System.Text;
using Jg.wpf.core.Utility;
using Microsoft.Win32;

namespace Jg.wpf.core.Service.FileService.FileTypes
{
    internal class JsonFileImp
    {
        public void Save(string path, object fileObject)
        {
            var strings = path.Split('\\');
            var fileName = strings[strings.Length - 1];
            var dir = path.Remove(path.Length - fileName.Length);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
            if (fileObject is string)
            {
                File.WriteAllText(path, fileObject.ToString(), Encoding.UTF8);
            }
            else
            {
                var json = fileObject.ToJson();
                File.WriteAllText(path, json, Encoding.UTF8);
            }
        }

        public bool SaveAs(string folderPath, object fileObject)
        {
            var extension = "json";
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = $"{extension} file|*.{extension}";
            saveFileDialog.FileName = "Save-" + DateTime.Now.ToString("yyyyMMddHHmm") + $".{extension}";
            if (!string.IsNullOrEmpty(folderPath))
            {
                if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
                saveFileDialog.InitialDirectory = folderPath;
            }

            var res = saveFileDialog.ShowDialog();
            if (res != null && res.Value)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, false);
                streamWriter.Write(fileObject is string ? fileObject.ToString() : fileObject.ToJson());
                streamWriter.Close();
                return true;
            }

            return false;
        }

        public T Load<T>(string path)
        {
            if (File.Exists(path))
            {
                var read = File.ReadAllText(path, Encoding.UTF8);
                return read.FromJson<T>();
            }

            return default;
        }

        public T LoadFromFolder<T>(string folderPath)
        {
            var extension = "json";
            var openFileDialog = new OpenFileDialog { Filter = $"{extension} file|*.{extension}|{extension} file|*.{extension}" };
            openFileDialog.InitialDirectory = folderPath;
            var res = openFileDialog.ShowDialog();
            if (res == null || !res.Value) { return default; }
            var reader = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
            return reader.FromJson<T>();
        }
    }
}
