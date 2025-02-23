using Jg.wpf.core.Extensions.Types.Pages;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class PageControlViewModel : ViewModelBase
    {
        private int _recordCount = 100;
        private int _pageSize = 10;
        private int _continuousCount = 3;
        private int _currentPage;


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
    }
}
