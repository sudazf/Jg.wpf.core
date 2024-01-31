using System;
using System.Windows.Media;
using System.Windows;
using Jg.wpf.core.Extensions.Types.RoiTypes;
using System.Globalization;

namespace Jg.wpf.controls.Customer.CustomImage
{
    public class RoiDrawingVisual : DrawingVisual
    {
        private readonly float _pixelPerDpi;
        private readonly BrushConverter _brushConverter = new BrushConverter();

        public RoiDrawingVisual(float pixelPerDpi)
        {
            _pixelPerDpi = pixelPerDpi;
        }

        public void DrawRoi(Roi roi, double scale, Thickness thickness)
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                try
                {
                    var leftThickness = thickness.Left;
                    var topThickness = thickness.Top;
                    var rightThickness = thickness.Right;
                    var bottomThickness = thickness.Bottom;

                    var emSize = 14 / scale;
                    emSize = Math.Round(emSize, 1);

                    var color = (SolidColorBrush)_brushConverter.ConvertFromString(roi.Color);
                    var leftPen = new Pen(color, leftThickness);
                    var topPen = new Pen(color, topThickness);
                    var rightPen = new Pen(color, rightThickness);
                    var bottomPen = new Pen(color, bottomThickness);

                    var leftD = leftPen.Thickness / 2;
                    var topD = topPen.Thickness / 2 ;
                    var rightD = rightPen.Thickness / 2;
                    var bottomD = bottomPen.Thickness / 2 ;

                    var bottomLeft = new Point(roi.X, roi.Y + roi.Height);
                    var topLeft = new Point(roi.X, roi.Y);
                    var topRight = new Point(roi.X + roi.Width, roi.Y);
                    var bottomRight = new Point(roi.X + roi.Width, roi.Y + roi.Height);

                    var leftLineStartPoint = new Point(bottomLeft.X, bottomLeft.Y + bottomD);
                    var leftLineEndPoint = new Point(topLeft.X, topLeft.Y - topD );

                    var topLineStartPoint = new Point(topLeft.X, topLeft.Y);
                    var topLineEndPoint = new Point(topRight.X , topRight.Y);

                    var rightLineStartPoint = new Point(topRight.X, topRight.Y - topD);
                    var rightLineEndPoint = new Point(bottomRight.X, bottomRight.Y + bottomD);

                    var bottomLineStartPoint = new Point(bottomRight.X, bottomRight.Y);
                    var bottomLineEndPoint = new Point(bottomLeft.X, bottomLeft.Y);

                    var leftGuidelines = new GuidelineSet(new[]
                    {
                        leftLineStartPoint.X
                    }, new[]
                    {
                        leftLineStartPoint.Y, leftLineEndPoint.Y,
                    });

                    var topGuidelines = new GuidelineSet(new[]
                    {
                        topLineStartPoint.X, topLineEndPoint.X
                    }, new[]
                    {
                        topLineStartPoint.Y
                    });
                    var rightGuidelines = new GuidelineSet(new[]
                    {
                        rightLineStartPoint.X
                    }, new[]
                    {
                        rightLineStartPoint.Y, rightLineEndPoint.Y
                    });
                    var bottomGuidelines = new GuidelineSet(new[]
                    {
                        bottomLineStartPoint.X, bottomLineEndPoint.X
                    }, new[]
                    {
                        bottomLineStartPoint.Y
                    });

                    dc.PushGuidelineSet(leftGuidelines);
                    dc.DrawLine(leftPen, leftLineStartPoint, leftLineEndPoint);

                    dc.PushGuidelineSet(topGuidelines);
                    dc.DrawLine(topPen, topLineStartPoint, topLineEndPoint);

                    dc.PushGuidelineSet(rightGuidelines);
                    dc.DrawLine(rightPen, rightLineStartPoint, rightLineEndPoint);

                    dc.PushGuidelineSet(bottomGuidelines);
                    dc.DrawLine(bottomPen, bottomLineStartPoint, bottomLineEndPoint);

                    if (!string.IsNullOrEmpty(roi.Title))
                    {
                        var titleText = new FormattedText(roi.Title, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                            new Typeface("宋体"), emSize, color, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                       
                        var titlePoint = new Point(roi.X, roi.Y + roi.Height + 10);
                        dc.DrawText(titleText, titlePoint);
                    }

                    if (roi.ShowRoiValue)
                    {
                        var xyValues = $"X:{roi.X} Y:{roi.Y}, W:{roi.Width} H:{roi.Height}";

                        var xyText = new FormattedText(xyValues, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                            new Typeface("宋体"), emSize, color, VisualTreeHelper.GetDpi(this).PixelsPerDip);

                        var xyStartPoint = new Point(roi.X, roi.Y + roi.Height + 10);

                        if (!string.IsNullOrEmpty(roi.Title))
                        {
                            var titleText = new FormattedText(roi.Title, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                                new Typeface("宋体"), emSize, color, VisualTreeHelper.GetDpi(this).PixelsPerDip);

                            xyStartPoint.X = xyStartPoint.X + titleText.Width + 4;
                        }

                        dc.DrawText(xyText, xyStartPoint);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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

        public void DrawEditor(Roi hitRoi, double scale)
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                var thickness = 1 / scale;
                thickness = Math.Round(thickness, 1);

                if (thickness < 1)
                {
                    thickness = 1;
                }
                if (thickness > 10)
                {
                    thickness = 10;
                }
                _editorPen.Thickness = thickness;

                var topLeft = new Point(hitRoi.X, hitRoi.Y);
                var bottomRight = new Point(hitRoi.X + hitRoi.Width, hitRoi.Y + hitRoi.Height);
                var topRight = new Point(bottomRight.X, topLeft.Y);
                var bottomLeft = new Point(topLeft.X, bottomRight.Y);
                var topCenter = new Point((topLeft.X + bottomRight.X) / 2, topLeft.Y);
                var bottomCenter = new Point((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
                var leftCenter = new Point(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
                var rightCenter = new Point(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);

                double radius = 12 / scale;
                radius = Math.Round(radius, 1);

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
        public void ClearEditor()
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                dc.DrawRectangle(Brushes.Transparent, _clearPen, 
                    new Rect(0,0,1,1));
            }
        }
    }

    public class RoiCreatorDrawingVisual : DrawingVisual
    {
        private readonly BrushConverter _brushConverter = new BrushConverter();
        private readonly Pen _clearPen = new Pen(Brushes.Transparent, 1);

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }


        public RoiCreatorDrawingVisual()
        {

        }

        public void DrawCreator(int x, int y, int width, int height, string stringColor, float thickness)
        {
            X = x;
            Y = y;
            Width = width; 
            Height = height;

            using (DrawingContext dc = this.RenderOpen())
            {
                try
                {
                    if (thickness < 1)
                    {
                        thickness = 1;
                    }
                    if (thickness > 10)
                    {
                        thickness = 10;
                    }

                    var color = (SolidColorBrush)_brushConverter.ConvertFromString(stringColor);
                    var pen = new Pen(color, thickness);
                    var topLeft = new Point(x, y);
                    var bottomRight = new Point(x + width, y + height);

                    var d = pen.Thickness / 2;
                    var guidelines = new GuidelineSet(new[] { d }, new[] { d });
                    dc.PushGuidelineSet(guidelines);

                    dc.DrawRectangle(Brushes.Transparent, pen,
                        new Rect(topLeft, bottomRight));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void Clear()
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                dc.DrawRectangle(Brushes.Transparent, _clearPen,
                    new Rect(0, 0, 1, 1));
            }
        }

        public void Reset()
        {
            X=0; 
            Y=0;
            Width = 0;
            Height = 0;
        }
    }
}
