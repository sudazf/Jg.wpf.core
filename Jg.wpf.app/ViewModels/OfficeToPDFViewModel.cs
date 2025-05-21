using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;
using System;
using Jg.wpf.core.Service;
using Jg.wpf.core.Service.FileService;

namespace Jg.wpf.app.ViewModels
{
    public class OfficeToPDFViewModel : ViewModelBase
    {
        private readonly IFileService _fileService;
        private string _wordSourceFile;
        private string _outputFolder;

        public string WordSourceFile
        {
            get => _wordSourceFile;
            set
            {
                if (value == _wordSourceFile) return;
                _wordSourceFile = value;
                RaisePropertyChanged(nameof(WordSourceFile));
            }
        }

        public string OutputFolder
        {
            get => _outputFolder;
            set
            {
                if (value == _outputFolder) return;
                _outputFolder = value;
                RaisePropertyChanged(nameof(OutputFolder));
            }
        }

        public JCommand GetWordFileCommand { get; }
        public JCommand GetOutputFolderCommand { get; }
        public JCommand WordToPDFCommand { get; }

        public OfficeToPDFViewModel()
        {
            _fileService = ServiceManager.GetService<IFileService>();

            GetOutputFolderCommand = new JCommand("GetOutputFolderCommand", OnGetOutputFolder);
            WordToPDFCommand = new JCommand("WordToPDFCommand", OnWordToPDF);
            GetWordFileCommand = new JCommand("GetWordFileCommand", OnGetWordFile);
        }

        private void OnGetOutputFolder(object obj)
        {
            OutputFolder = _fileService.GetFolder();
        }

        private void OnGetWordFile(object obj)
        {
            WordSourceFile = _fileService.GetFile();
        }

        private void OnWordToPDF(object obj)
        {
            try
            {
                //if (string.IsNullOrEmpty(WordSourceFile) || string.IsNullOrEmpty(OutputFolder))
                //{
                //    MessageBox.Show("need file path.");
                //    return;
                //}

                //Document doc = new Document(WordSourceFile);
                ////保存为PDF文件，此处的SaveFormat支持很多种格式，如图片，epub,rtf 等等

                ////权限这块的设置成不可复制
                //PdfSaveOptions saveOptions = new PdfSaveOptions();
                //// Create encryption details and set owner password.
                //PdfEncryptionDetails encryptionDetails = new PdfEncryptionDetails(string.Empty, "password",
                //    PdfPermissions.AllowAll);
                //// Start by disallowing all permissions.
                //encryptionDetails.Permissions = PdfPermissions.DisallowAll;
                //// Extend permissions to allow editing or modifying annotations.
                //encryptionDetails.Permissions = PdfPermissions.ModifyAnnotations | PdfPermissions.DocumentAssembly;
                //saveOptions.EncryptionDetails = encryptionDetails;
                //// Render the document to PDF format with the specified permissions.
                //var outputFile = Path.Combine(OutputFolder, "output.pdf");
                //doc.Save(outputFile, saveOptions);

                //System.Diagnostics.Process.Start(outputFile);

                ////doc.Save(to, SaveFormat.Pdf);
                //Console.WriteLine("成功!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
