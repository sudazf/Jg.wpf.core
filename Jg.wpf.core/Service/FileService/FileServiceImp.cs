using Jg.wpf.core.Service.FileService.FileTypes;
using Microsoft.WindowsAPICodePack.Dialogs;

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

        public void Save(string filePath, object fileObject, FileType fileType)
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

        public void SaveAs(object fileObject, FileType fileType, string folderPath)
        {
            switch (fileType)
            {
                case FileType.Txt:
                    _txtFileImp.SaveAs(fileType, folderPath);
                    break;
                case FileType.Json:
                    _jsonFileImp.SaveAs(fileType, folderPath);
                    break;
                case FileType.Binary:
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

        public string GetFile(string suffix)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = false;
            if (!string.IsNullOrEmpty(suffix))
            {
                dialog.Filters.Add(new CommonFileDialogFilter($"{suffix.ToUpper()} Files", $"*.{suffix}"));
            }
            else
            {
                dialog.Filters.Add(new CommonFileDialogFilter("所有文件 Files", "*.*"));
            }

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (string.IsNullOrEmpty(dialog.FileName))
                {
                    return "";
                }

                return dialog.FileName;
            }

            return "";
        }

        public string GetFolder(string title)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Title = title;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (string.IsNullOrEmpty(dialog.FileName))
                {
                    return "";
                }

                return dialog.FileName;
            }

            return "";
        }
    }
}
