namespace Jg.wpf.core.Service.FileService
{
    public interface IFileDialogService
    {
        string Save(string format);
        string Open(string format);
    }

}
