using System.Windows.Media;
using System.Windows;

namespace Jg.wpf.controls.Customer.CustomImage
{
    public class RoiDrawingVisual : DrawingVisual
    {
        private readonly BrushConverter _brushConverter = new BrushConverter();
        public RoiDrawingVisual()
        {

        }

        public void DrawRoi(Roi roi)
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                var color = (SolidColorBrush)_brushConverter.ConvertFromString(roi.Color);
                var pen = new Pen(color, 1);
                var topLeft = new Point(roi.X, roi.Y);
                var bottomRight = new Point(roi.X + roi.Width,roi.Y + roi.Height);

                dc.DrawRectangle(Brushes.Transparent, pen,
                    new Rect(topLeft, bottomRight));
            }
        }
    }

    public class RoiEditorDrawingVisual : DrawingVisual
    {
        private readonly Pen _editorPen = new Pen(Brushes.Gray,1);
        private readonly Pen _clearPen = new Pen(Brushes.Transparent,1);
        public RoiEditorDrawingVisual()
        {

        }

        public void DrawEditor(Roi hitRoi)
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                var topLeft = new Point(hitRoi.X, hitRoi.Y);
                var bottomRight = new Point(hitRoi.X + hitRoi.Width, hitRoi.Y + hitRoi.Height);
                var topRight = new Point(bottomRight.X, topLeft.Y);
                var bottomLeft = new Point(topLeft.X, bottomRight.Y);
                var topCenter = new Point((topLeft.X + bottomRight.X) / 2, topLeft.Y);
                var bottomCenter = new Point((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
                var leftCenter = new Point(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
                var rightCenter = new Point(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);

                dc.DrawEllipse(Brushes.LightGray, _editorPen, topLeft, 5, 5);
                dc.DrawEllipse(Brushes.LightGray, _editorPen, bottomRight, 5, 5);
                dc.DrawEllipse(Brushes.LightGray, _editorPen, topRight, 5, 5);
                dc.DrawEllipse(Brushes.LightGray, _editorPen, bottomLeft, 5, 5);
                dc.DrawEllipse(Brushes.LightGray, _editorPen, topCenter, 5, 5);
                dc.DrawEllipse(Brushes.LightGray, _editorPen, bottomCenter, 5, 5);
                dc.DrawEllipse(Brushes.LightGray, _editorPen, leftCenter, 5, 5);
                dc.DrawEllipse(Brushes.LightGray, _editorPen, rightCenter, 5, 5);
            }
        }
        public void ClearEditor(Roi hitRoi)
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                var topLeft = new Point(hitRoi.X, hitRoi.Y);
                var bottomRight = new Point(hitRoi.X + hitRoi.Width, hitRoi.Y + hitRoi.Height);
                var topRight = new Point(bottomRight.X, topLeft.Y);
                var bottomLeft = new Point(topLeft.X, bottomRight.Y);
                var topCenter = new Point((topLeft.X + bottomRight.X) / 2, topLeft.Y);
                var bottomCenter = new Point((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
                var leftCenter = new Point(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
                var rightCenter = new Point(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);

                dc.DrawEllipse(Brushes.Transparent, _clearPen, topLeft, 5, 5);
                dc.DrawEllipse(Brushes.Transparent, _clearPen, bottomRight, 5, 5);
                dc.DrawEllipse(Brushes.Transparent, _clearPen, topRight, 5, 5);
                dc.DrawEllipse(Brushes.Transparent, _clearPen, bottomLeft, 5, 5);
                dc.DrawEllipse(Brushes.Transparent, _clearPen, topCenter, 5, 5);
                dc.DrawEllipse(Brushes.Transparent, _clearPen, bottomCenter, 5, 5);
                dc.DrawEllipse(Brushes.Transparent, _clearPen, leftCenter, 5, 5);
                dc.DrawEllipse(Brushes.Transparent, _clearPen, rightCenter, 5, 5);
            }
        }
    }
}
