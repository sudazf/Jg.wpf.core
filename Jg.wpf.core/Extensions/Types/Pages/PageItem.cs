using System;
using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;

namespace Jg.wpf.core.Extensions.Types.Pages
{
    public class PageItem : ViewModelBase
    {
        private int _type;
        private bool _isCurrentPage;
        private string _currentPageColor = "Green";
        private int _page;
        private bool _numVisible;
        private bool _omitVisible;

        public event EventHandler<PageChangedEventArgs> OnPageChanged;

        public int Type
        {
            get => _type;
            set
            {
                if (value == _type) return;
                _type = value;

                if (_type == 1)
                {
                    NumVisible = true;
                    OmitVisible = false;
                }
                else
                {
                    NumVisible = false;
                    OmitVisible = true;
                }
                RaisePropertyChanged(nameof(Type));
            }
        }

        public bool IsCurrentPage
        {
            get => _isCurrentPage;
            set
            {
                if (value == _isCurrentPage) return;
                _isCurrentPage = value;
                if (_isCurrentPage)
                {
                    CurrentPageColor = "Blue";
                }
                else
                {
                    CurrentPageColor = "Red";
                }
                RaisePropertyChanged(nameof(IsCurrentPage));
            }
        }

        public string CurrentPageColor
        {
            get => _currentPageColor;
            set
            {
                if (value == _currentPageColor) return;
                _currentPageColor = value;
                RaisePropertyChanged(nameof(CurrentPageColor));
            }
        }

        public int Page
        {
            get => _page;
            set
            {
                if (value == _page) return;
                _page = value;
                RaisePropertyChanged(nameof(Page));
            }
        }

        public bool NumVisible
        {
            get => _numVisible;
            set
            {
                if (value == _numVisible) return;
                _numVisible = value;
                RaisePropertyChanged(nameof(NumVisible));
            }
        }

        public bool OmitVisible
        {
            get => _omitVisible;
            set
            {
                if (value == _omitVisible) return;
                _omitVisible = value;
                RaisePropertyChanged(nameof(OmitVisible));
            }
        }

        public JCommand PageClickCommand { get;}

        public PageItem(int page, int currentPage, int type = 1)
        {
            Type = type;
            Page = page;
            IsCurrentPage = page == currentPage;

            PageClickCommand = new JCommand("PageClickCommand", OnClick);
        }

        private void OnClick(object obj)
        {
            OnPageChanged?.Invoke(this, new PageChangedEventArgs(Page));
        }
    }
}
