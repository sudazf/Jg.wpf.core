using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Jg.wpf.core.Extensions.Types.Pages;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class PageControlViewModel : ViewModelBase
    {
        private int _pageCount;
        private int _recordCount;
        private int _pageSize;
        private int _continuousCount;
        private int _currentPage;

        public int PageCount
        {
            get => _pageCount;
            set
            {
                if (value == _pageCount) return;
                _pageCount = value;
                RaisePropertyChanged(nameof(PageCount));
            }
        }

        public int RecordCount
        {
            get => _recordCount;
            set
            {
                if (value == _recordCount) return;
                _recordCount = value;
                RaisePropertyChanged(nameof(RecordCount));
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value == _pageSize) return;
                _pageSize = value;
                RaisePropertyChanged(nameof(PageSize));
            }
        }

        public int ContinuousCount
        {
            get => _continuousCount;
            set
            {
                if (value == _continuousCount) return;
                _continuousCount = value;
                RaisePropertyChanged(nameof(ContinuousCount));
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (value == _currentPage) return;
                _currentPage = value;
                RaisePropertyChanged(nameof(CurrentPage));
            }
        }

        public PageControlViewModel()
        {

        }
     

        private void OnPageChanged(object sender, PageChangedEventArgs e)
        {
            //todo
        }
    }
}
