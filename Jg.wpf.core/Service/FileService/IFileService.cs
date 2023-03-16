namespace Jg.wpf.core.Service.FileService
{
    public interface IFileService
    {
        void Save(string filePath, object fileObject, FileType type);
        void SaveAs(object fileObject, FileType fileType, string folderPath);

        T Load<T>(string filePath, FileType fileType);
        T LoadFromFolder<T>(string folderPath, FileType fileType);

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="suffix">文件后缀名</param>
        /// <returns>文件全路径</returns>
        string GetFile(string suffix);
        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="title">选择框标题</param>
        /// <returns>文件夹全路径</returns>
        string GetFolder(string title);
    }

    public enum FileType
    {
        Txt,
        Json,
        Binary,
        Xml,
        Csv
    }
}
