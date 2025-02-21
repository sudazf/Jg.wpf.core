using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class TabControlViewModel : ViewModelBase
    {
        public ObservableCollection<string> TestItems { get; }

        public JCommand AddCommand { get;  }

        public TabControlViewModel()
        {
            TestItems = new ObservableCollection<string>()
            {
                "AAAAAAAAAA", "BBBBBBBBB"
            };

            AddCommand = new JCommand("AddCommand", OnAdd);
        }

        private void OnAdd(object obj)
        {
            TestItems.Add("12345");
        }
    }
}
