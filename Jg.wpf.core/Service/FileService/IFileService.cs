namespace Jg.wpf.core.Service.FileService
{
    public interface IFileService
    {
        string FileName { get; }
        T Load<T>(string path, FileType fileType = FileType.Txt);
        object LoadFrom(FileType fileType = FileType.Txt, string dicPath = "");
        void Save(string path, object file, FileType fileType = FileType.Txt);
        void SaveAs(object file, FileType fileType = FileType.Txt);

        void AppendLines(string path, string text, FileType fileType = FileType.Txt);
    }
    public enum FileType
    {
        Txt,
        Json
    }
}
