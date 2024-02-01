namespace Jg.wpf.core.Extensions.Types.RoiTypes
{
    public class RoiThickness
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }

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
