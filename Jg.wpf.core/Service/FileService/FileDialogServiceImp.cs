using System;
using Microsoft.Win32;

namespace Jg.wpf.core.Service.FileService
{
    internal class FileDialogServiceImp : IFileDialogService
    {
        public string Save(string format)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = $"{format} file|*.{format}",
                FileName = "Solar-" + DateTime.Now.ToString("yyyyMMddHHmm") + $".{format}"
            };
            var res = saveFileDialog.ShowDialog();
            if (res != null && res.Value)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

        public string Open(string format)
        {
            var openFileDialog = new OpenFileDialog { Filter = $"{format} file|*.{format}" };
            var res = openFileDialog.ShowDialog();
            if (res == null || !res.Value)
                return null;

            return openFileDialog.FileName;
        }
    }
}
