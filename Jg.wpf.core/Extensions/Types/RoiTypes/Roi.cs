using Jg.wpf.core.Notify;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jg.wpf.core.Extensions.Types.RoiTypes
{
    public class Roi : ViewModelBase, IDisposable
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        private bool _show;
        private string _title;
        private bool _showRoiValue;
        private string _color;
        private RoiThickness _thickness;
        private bool _disposed;
        private RoiRestrictedTypes _restrictedType = RoiRestrictedTypes.None;

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
        public string Color
        {
            get => _color;
            set
            {
                if (value == _color) return;
                _color = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(Color));
            }
        }
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
        public RoiThickness Thickness
        {
            get => _thickness;
            set
            {
                if (Equals(value, _thickness)) return;
                _thickness = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(Thickness));
            }
        }
        public RoiRestrictedTypes RestrictedType
        {
            get => _restrictedType;
            set
            {
                if (value == _restrictedType) return;
                _restrictedType = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(RestrictedType));
            }
        }

        public List<string> Colors { get; private set; }
        public bool CanOverLaid { get; set; }
        public IList<RoiRestrictedTypes> RestrictedTypes { get; set; }
        

        public Roi(int x, int y, int width, int height, 
            string color, bool show = true, string title = null, bool showRoiValue = true,
            List<string> colors = null, bool canOverLaid = true, 
            RoiRestrictedTypes restrictedType = RoiRestrictedTypes.None)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;

            Color = color;
            Show = show;
            Title = title;

            ShowRoiValue = showRoiValue;

            Colors = new List<string>();

            if (colors != null)
            {
                foreach (var supportColor in colors)
                {
                    Colors.Add(supportColor);
                }

                if (Colors.Count <= 0)
                {
                    ProvideDefaultColors();
                }
            }
            else
            {
                ProvideDefaultColors();
            }

            CanOverLaid = canOverLaid;
            RestrictedTypes = new List<RoiRestrictedTypes>()
            { 
                 RoiRestrictedTypes.None,
                 RoiRestrictedTypes.X,
                 RoiRestrictedTypes.Y,
            };

            _restrictedType = RestrictedTypes.FirstOrDefault(r => r == restrictedType);

            Thickness = new RoiThickness(2);
            Thickness.OnSingleThicknessChanged += OnSingleThicknessChanged;
        }


        public bool Hit(JPoint point)
        {
            return Contains(point) && Show;
        }
        public void Update(int x, int y, int width, int height)
        {
            switch (RestrictedType)
            {
                case RoiRestrictedTypes.None:
                    _x = x;
                    _y = y;
                    _width = width;
                    _height = height;
                    break;
                case RoiRestrictedTypes.X:
                    _y = y;
                    _height = height;
                    break;
                case RoiRestrictedTypes.Y:
                    _x = x;
                    _width = width;
                    break;
            }

            RaisePropertyChanged(nameof(X));
            RaisePropertyChanged(nameof(Y));
            RaisePropertyChanged(nameof(Width));
            RaisePropertyChanged(nameof(Height));

            OnRoiChanged?.Invoke(this, this);
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                Thickness.OnSingleThicknessChanged -= OnSingleThicknessChanged;

                _disposed = true;
            }
        }

        private bool Contains(JPoint point)
        {
            var x = point.X;
            var y = point.Y;
            return ((x >= _x - 5) && (x - _width <= _x + 5) &&
                    (y >= _y - 5) && (y - _height <= _y + 5));
        }
        private void ProvideDefaultColors()
        {
            Colors = new List<string>
            {
                "Red", "Green", "Blue", "White", 
                "Black", "Yellow", "Crimson", "DeepPink", 
                "DarkOrange", "LightGreen"
            };
        }
        private void OnSingleThicknessChanged(object sender, RoiThickness e)
        {
            OnRoiChanged?.Invoke(this, this);
        }
    }
}
