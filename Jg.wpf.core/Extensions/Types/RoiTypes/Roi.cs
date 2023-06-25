using Jg.wpf.core.Notify;
using System;

namespace Jg.wpf.core.Extensions.Types.RoiTypes
{
    public class Roi : ViewModelBase
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        private bool _show;
        private string _title;
        private bool _showRoiValue;

        public event EventHandler<Roi> OnRoiChanged;

        public int X
        {
            get => _x;
            set
            {
                if (value == _x) return;
                _x = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(X));
            }
        }
        public int Y
        {
            get => _y;
            set
            {
                if (value == _y) return;
                _y = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(Y));
            }
        }
        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(Width));
            }
        }
        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(Height));
            }
        }
        public bool Show
        {
            get => _show;
            set
            {
                if (value == _show) return;
                _show = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(Show));
            }
        }
        public string Color { get; set; }
        public string Title
        {
            get => _title;
            set
            {
                if (value == _title) return;
                _title = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(Title));
            }
        }

        public bool ShowRoiValue
        {
            get => _showRoiValue;
            set
            {
                if (value == _showRoiValue) return;
                _showRoiValue = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(() => ShowRoiValue);
            }
        }

        public Roi(int x, int y, int width, int height, 
            string color, bool show = true, string title = null, bool showRoiValue = true)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;

            Color = color;
            Show = show;
            Title = title;

            ShowRoiValue = showRoiValue;
        }

        public bool Hit(JPoint point)
        {
            return Contains(point) && Show;
        }
        public void Update(int x, int y, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;

            RaisePropertyChanged(nameof(X));
            RaisePropertyChanged(nameof(Y));
            RaisePropertyChanged(nameof(Width));
            RaisePropertyChanged(nameof(Height));

            OnRoiChanged?.Invoke(this, this);
        }

        private bool Contains(JPoint point)
        {
            var x = point.X;
            var y = point.Y;
            return ((x >= _x - 5) && (x - _width <= _x + 5) &&
                    (y >= _y - 5) && (y - _height <= _y + 5));
        }
    }
}
