using System;

namespace Jg.wpf.core.Extensions.Types.RoiTypes
{
    public class RoiThickness
    {
        public event EventHandler<RoiThickness> OnSingleThicknessChanged;

        private float _left;
        private float _top;
        private float _right;
        private float _bottom;

        public float Left
        {
            get => _left;
            set
            {
                if (Math.Abs(_left - value) > 0)
                {
                    _left = value;
                    OnSingleThicknessChanged?.Invoke(this, this);
                }
            }
        }

        public float Top
        {
            get => _top;
            set
            {
                if (Math.Abs(_top - value) > 0)
                {
                    _top = value;
                    OnSingleThicknessChanged?.Invoke(this, this);
                }
            }
        }

        public float Right
        {
            get => _right;
            set
            {
                if (Math.Abs(_right - value) > 0)
                {
                    _right = value;
                    OnSingleThicknessChanged?.Invoke(this, this);
                }
            }
        }

        public float Bottom
        {
            get => _bottom;
            set
            {
                if (Math.Abs(_bottom - value) > 0)
                {
                    _bottom = value;
                    OnSingleThicknessChanged?.Invoke(this, this);
                }
            }
        }


        public RoiThickness()
        {
            
        }

        public RoiThickness(float all)
        {
            Left = all;
            Top = all;
            Right = all;
            Bottom = all;
        }

        public RoiThickness(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

    }
}
