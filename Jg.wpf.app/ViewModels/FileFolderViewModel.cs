using Jg.wpf.core.Command;
using Jg.wpf.core.Service.FileService;
using Jg.wpf.core.Service;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class FileFolderViewModel : ViewModelBase
    {
        private readonly IFileService _fileService;
        private string _folderName;
        private string _fileName;

        public JCommand GetFolderCommand { get; }

        public string FolderName
        {
            get => _folderName;
            set
            {
                _folderName = value;
                RaisePropertyChanged(nameof(FolderName));
            }
        }

        public JCommand GetFileNameCommand { get; }

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                RaisePropertyChanged(nameof(FileName));
            }
        }

        public FileFolderViewModel()
        {
            _fileService = ServiceManager.GetService<IFileService>();

            GetFolderCommand = new JCommand("GetFolderCommand", OnGetFolder);
            GetFileNameCommand = new JCommand("GetFileNameCommand", OnGetFile);
        }

        private void OnGetFolder(object obj)
        {
            FolderName = _fileService.GetFolder();
        }
        private void OnGetFile(object obj)
        {
            FileName = _fileService.GetFile();
        }
    }
}
