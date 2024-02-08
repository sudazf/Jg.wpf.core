using Jg.wpf.core.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using Jg.wpf.core.Utility;

namespace Jg.wpf.core.Extensions.Types.RoiTypes
{
    public class Roi : ViewModelBase, IDisposable
    {
        private double _x;
        private double _y;
        private double _width;
        private double _height;
        private bool _show;
        private string _title;
        private bool _showRoiValue;
        private string _color;
        private RoiThickness _thickness;
        private bool _disposed;
        private RoiRestrictedTypes _restrictedType;

        public event EventHandler<Roi> OnRoiChanged;

        public double X
        {
            get => _x;
            set
            {
                if (Math.Abs(value - _x) < 0.00001) return;
                _x = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(X));
            }
        }
        public double Y
        {
            get => _y;
            set
            {
                if (Math.Abs(value - _y) < 0.00001) return;
                _y = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(Y));
            }
        }
        public double Width
        {
            get => _width;
            set
            {
                if (Math.Abs(value - _width) < 0.00001) return;
                _width = value;
                OnRoiChanged?.Invoke(this, this);
                RaisePropertyChanged(nameof(Width));
            }
        }
        public double Height
        {
            get => _height;
            set
            {
                if (Math.Abs(value - _height) < 0.00001) return;
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

        public Roi(double x, double y, double width, double height, 
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
        public void Update(double x, double y, double width, double height)
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
    

            OnRoiChanged?.Invoke(this, this);

            _x = Math.Round(_x);
            _y = Math.Round(_y);
            _width = Math.Round(_width);
            _height = Math.Round(_height);

            RaisePropertyChanged(nameof(X));
            RaisePropertyChanged(nameof(Y));
            RaisePropertyChanged(nameof(Width));
            RaisePropertyChanged(nameof(Height));
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
            Colors = (List<string>)JColorHelper.GetSysColors();
        }
        private void OnSingleThicknessChanged(object sender, RoiThickness e)
        {
            OnRoiChanged?.Invoke(this, this);
        }
    }
}
