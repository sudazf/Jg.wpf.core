using System.Windows.Media;
using System.Windows;
using Jg.wpf.core.Extensions.Types.RoiTypes;
using System.Globalization;

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

                var d = pen.Thickness / 2;
                var guidelines2 = new GuidelineSet(new[] { d }, new[] { d });
                dc.PushGuidelineSet(guidelines2);

                dc.DrawRectangle(Brushes.Transparent, pen,
                    new Rect(topLeft, bottomRight));

                var titleText = new FormattedText(roi.Title, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                    new Typeface("宋体"), 14, color, VisualTreeHelper.GetDpi(this).PixelsPerDip);

                if (!string.IsNullOrEmpty(roi.Title))
                {
                    var titlePoint = new Point(roi.X, roi.Y + roi.Height + 10);
                    dc.DrawText(titleText, titlePoint);
                }

                if (roi.ShowRoiValue)
                {
                    var xyValues = $"X:{roi.X} Y:{roi.Y}, W:{roi.Width} H:{roi.Height}";

                    var xyText = new FormattedText(xyValues, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        new Typeface("宋体"), 14, color, VisualTreeHelper.GetDpi(this).PixelsPerDip);
        
                    var xyStartPoint = new Point(roi.X, roi.Y + roi.Height + 10);

                    if (!string.IsNullOrEmpty(roi.Title))
                    {
                        xyStartPoint.X = xyStartPoint.X + titleText.Width + 4;
                    }

                    dc.DrawText(xyText, xyStartPoint);
                }
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

                double radius = 12;
                var d = _editorPen.Thickness / 2;
                var guidelines = new GuidelineSet(new[] {
                        d,
                        topLeft.X - radius / 2 + d,  topLeft.X + radius / 2 + d,
                        topCenter.X - radius / 2 + d, topCenter.X + radius / 2 + d,
                        topRight.X - radius / 2 + d,  topRight.X + radius / 2 + d,
                        leftCenter.X - radius / 2 + d,  leftCenter.X + radius / 2 + d,
                        rightCenter.X - radius / 2 + d,  rightCenter.X + radius / 2 + d,
                        bottomLeft.X - radius / 2 + d,  bottomLeft.X + radius / 2 + d,
                        bottomCenter.X - radius / 2 + d,  bottomCenter.X + radius / 2 + d,
                        bottomRight.X - radius / 2 + d,  bottomRight.X + radius / 2 + d,
                    },
                    new[]
                    {
                        d,
                        topLeft.Y - radius / 2 + d,  topLeft.Y + radius / 2 + d,
                        topCenter.Y - radius / 2 + d, topCenter.Y + radius / 2 + d,
                        topRight.Y - radius / 2 + d,  topRight.Y + radius / 2 + d,
                        leftCenter.Y - radius / 2 + d,  leftCenter.Y + radius / 2 + d,
                        rightCenter.Y - radius / 2 + d,  rightCenter.Y + radius / 2 + d,
                        bottomLeft.Y - radius / 2 + d,  bottomLeft.Y + radius / 2 + d,
                        bottomCenter.Y - radius / 2 + d,  bottomCenter.Y + radius / 2 + d,
                        bottomRight.Y - radius / 2 + d,  bottomRight.Y + radius / 2 + d,
                    });

                dc.PushGuidelineSet(guidelines);

                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(topLeft.X - radius / 2, topLeft.Y - radius / 2, radius, radius));
                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(topCenter.X - radius / 2, topCenter.Y - radius / 2, radius, radius));
                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(topRight.X - radius / 2, topRight.Y - radius / 2, radius, radius));
                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(leftCenter.X - radius / 2, leftCenter.Y - radius / 2, radius, radius));
                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(rightCenter.X - radius / 2, rightCenter.Y - radius / 2, radius, radius));
                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(bottomLeft.X - radius / 2,bottomLeft.Y - radius / 2, radius, radius));
                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(bottomCenter.X - radius / 2, bottomCenter.Y - radius / 2, radius, radius));
                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(bottomRight.X - radius / 2, bottomRight.Y - radius / 2, radius, radius));
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
