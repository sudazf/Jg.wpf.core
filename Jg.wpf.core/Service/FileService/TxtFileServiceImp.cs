using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jg.wpf.core.Utility;

namespace Jg.wpf.core.Service.FileService
{
    internal class TxtFileServiceImp : IFileService
    {
        private readonly string _condConfigPath = $"{AppDomain.CurrentDomain.BaseDirectory}Configs";

        public string FileName { get; private set; }

        public object LoadFrom(FileType fileType = FileType.Txt, string dicPath = "")
        {
            switch (fileType)
            {
                case FileType.Txt:
                case FileType.Json:
                    return LoadFromJsonFile(dicPath);
            }
            return null;
        }
        public T Load<T>(string path, FileType fileType = FileType.Txt)
        {
            FileName = path;
            var read = string.Empty;
            switch (fileType)
            {
                case FileType.Txt:
                case FileType.Json:
                    if (File.Exists(path)) read = File.ReadAllText(path, Encoding.UTF8);
                    break;
            }
            return read.FromJson<T>();
        }
        public void Save(string path, object fileObject, FileType fileType = FileType.Txt)
        {
            switch (fileType)
            {
                case FileType.Txt:
                case FileType.Json:
                    SaveToJson(path, fileObject);
                    break;
            }
        }
        public void SaveAs(object fileObject, FileType fileType = FileType.Txt)
        {
            switch (fileType)
            {
                case FileType.Txt:
                case FileType.Json:
                    SaveAsToJson(fileObject);
                    break;
            }
        }

        public void AppendLines(string path, string text, FileType fileType = FileType.Txt)
        {
            switch (fileType)
            {
                case FileType.Txt:
                case FileType.Json:
                    AppendAllLines(path, text);
                    break;
            }
        }

        private void AppendAllLines(string path, string text)
        {
            File.AppendAllLines(path, new List<string> { text }, Encoding.UTF8);
        }


        private object LoadFromJsonFile(string dicPath)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "json file|*.json",
                InitialDirectory = dicPath
            };
            var res = openFileDialog.ShowDialog();
            if (res == null || !res.Value) return null;
            FileName = openFileDialog.FileName;
            return File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
        }
        private void SaveToJson(string path, object fileObject)
        {
            if (!Directory.Exists(_condConfigPath))
            {
                Directory.CreateDirectory(_condConfigPath);
            }

            FileName = path;
            var json = fileObject.ToJson();
            File.WriteAllText(path, json, Encoding.UTF8);
        }
        private void SaveAsToJson(object fileObject)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "json file|*.json";
            saveFileDialog.FileName = "Cond-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".json";
            var res = saveFileDialog.ShowDialog();
            if (res != null && res.Value)
            {
                FileName = saveFileDialog.FileName;
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, false);
                streamWriter.Write(fileObject.ToJson());
                streamWriter.Close();
            }
        }
    }
}
