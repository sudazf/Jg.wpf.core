using Jg.wpf.core.Service.FileService.Custom;
using Jg.wpf.core.Service.FileService.FileTypes;
using Microsoft.Win32;
using System.IO;

namespace Jg.wpf.core.Service.FileService
{
    internal class FileServiceImp : IFileService
    {
        private readonly TxtFileImp _txtFileImp;
        private readonly JsonFileImp _jsonFileImp;
        private readonly BinaryFileImp _binaryFileImp;
        private readonly XmlFileImp _xmlFileImp;

        public FileServiceImp()
        {
            _jsonFileImp = new JsonFileImp();
            _txtFileImp = new TxtFileImp();
            _binaryFileImp = new BinaryFileImp();
            _xmlFileImp = new XmlFileImp();
        }

        public void Save<T>(string filePath, T fileObject, FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Txt:
                    _txtFileImp.Save(filePath, fileObject);
                    break;
                case FileType.Json:
                    _jsonFileImp.Save(filePath, fileObject);
                    break;
                case FileType.Binary:
                    _binaryFileImp.Save(filePath, fileObject);
                    break;
                case FileType.Xml:
                    _xmlFileImp.Save(filePath, fileObject);
                    break;
                case FileType.Csv:
                    break;
            }
        }

        public void SaveAs<T>(T fileObject, FileType fileType, string folderPath)
        {
            switch (fileType)
            {
                case FileType.Txt:
                    _txtFileImp.SaveAs(folderPath, fileObject);
                    break;
                case FileType.Json:
                    _jsonFileImp.SaveAs(folderPath, fileObject);
                    break;
                case FileType.Binary:
                    _binaryFileImp.SaveAs(folderPath, fileObject);
                    break;
                case FileType.Xml:
                    break;
                case FileType.Csv:
                    break;
            }
        }

        public T Load<T>(string filePath, FileType fileType)
        {
            T data = default(T);

            switch (fileType)
            {
                case FileType.Txt:
                    data = _txtFileImp.Load<T>(filePath);
                    break;
                case FileType.Json:
                    data = _jsonFileImp.Load<T>(filePath);
                    break;
                case FileType.Binary:
                    data = _binaryFileImp.Load<T>(filePath);
                    break;
                case FileType.Xml:
                    data = _xmlFileImp.Load<T>(filePath);
                    break;
                case FileType.Csv:
                    break;
            }

            return data;
        }

        public T LoadFromFolder<T>(string folderPath, FileType fileType)
        {
            T data = default(T);

            switch (fileType)
            {
                case FileType.Txt:
                    data = _txtFileImp.LoadFromFolder<T>(folderPath);
                    break;
                case FileType.Json:
                    data = _jsonFileImp.LoadFromFolder<T>(folderPath);
                    break;
                case FileType.Binary:
                    break;
                case FileType.Xml:
                    break;
                case FileType.Csv:
                    break;
            }

            return data;
        }

        public string GetFile(string suffix = "")
        {
            var dialog = new OpenFileDialog();

            if (string.IsNullOrEmpty(suffix))
            {
                dialog.Filter = "all file|*.*";
            }
            else
            {
                dialog.Filter = $"{suffix} file|*.{suffix}|all file|*.*";
            }

            if (dialog.ShowDialog().HasValue)
            {
                if (string.IsNullOrEmpty(dialog.FileName))
                {
                    return "";
                }

                return dialog.FileName;
            }

            return "";
        }

        public string GetFolder()
        {
            var dlg = new FolderPicker
            {
                InputPath = System.AppDomain.CurrentDomain.BaseDirectory
            };

            if (dlg.ShowDialog() == true)
            {
                return dlg.ResultPath;
            }

            return "";
        }
    }
}
