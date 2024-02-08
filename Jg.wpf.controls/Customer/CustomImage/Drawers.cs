using System;
using System.Windows.Media;
using System.Windows;
using Jg.wpf.core.Extensions.Types.RoiTypes;
using System.Reflection;
using System.Globalization;

namespace Jg.wpf.controls.Customer.CustomImage
{
    public class RoiDrawingVisual : DrawingVisual
    {
        private readonly BrushConverter _brushConverter = new BrushConverter();
        private readonly float _physicPixel;
        public RoiDrawingVisual()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

            var dpiX = (int)dpiXProperty.GetValue(null, null);
            var dpiY = (int)dpiYProperty.GetValue(null, null);

            var pixelsPerDpi = (float)dpiX / 96;
            _physicPixel = 1 / pixelsPerDpi;
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
                    if (leftThickness % _physicPixel != 0)
                    {
                        var intScale = Math.Round(leftThickness / _physicPixel);
                        leftThickness = intScale * _physicPixel;
                    }
                    if (topThickness % _physicPixel != 0)
                    {
                        var intScale = Math.Round(topThickness / _physicPixel);
                        topThickness = intScale * _physicPixel;
                    }
                    if (rightThickness % _physicPixel != 0)
                    {
                        var intScale = Math.Round(rightThickness / _physicPixel);
                        rightThickness = intScale * _physicPixel;
                    }
                    if (bottomThickness % _physicPixel != 0)
                    {
                        var intScale = Math.Round(bottomThickness / _physicPixel);
                        bottomThickness = intScale * _physicPixel;
                    }

                    var emSize = 14 / scale;
                    emSize = Math.Round(emSize, 1);

                    var color = (SolidColorBrush)_brushConverter.ConvertFromString(roi.Color);

                    var leftPen = new Pen(color, leftThickness);
                    var topPen = new Pen(color, topThickness);
                    var rightPen = new Pen(color, rightThickness);
                    var bottomPen = new Pen(color, bottomThickness);

                    var leftD = leftPen.Thickness / 2;
                    var topD = topPen.Thickness / 2;
                    var rightD = rightPen.Thickness / 2;
                    var bottomD = bottomPen.Thickness / 2;

                    var x = roi.X;
                    var y = roi.Y;
                    var w = roi.Width;
                    var h = roi.Height;

                    if (x % _physicPixel != 0)
                    {
                        var intScale = Math.Round(x / _physicPixel);
                        x = intScale * _physicPixel;
                    }
                    if (y % _physicPixel != 0)
                    {
                        var intScale = Math.Round(y / _physicPixel);
                        y = intScale * _physicPixel;
                    }
                    if (w % _physicPixel != 0)
                    {
                        var intScale = Math.Round(w / _physicPixel);
                        w = intScale * _physicPixel;
                    }
                    if (h % _physicPixel != 0)
                    {
                        var intScale = Math.Round(h / _physicPixel);
                        h = intScale * _physicPixel;
                    }

                    var bottomLeft = new Point(x, y + h);
                    var topLeft = new Point(x, y);
                    var topRight = new Point(x + w, y);
                    var bottomRight = new Point(x + w, y + h);
        
                    var leftLineStartPoint = new Point(bottomLeft.X, bottomLeft.Y + bottomD);
                    var leftLineEndPoint = new Point(topLeft.X, topLeft.Y - topD);

                    var topLineStartPoint = new Point(topLeft.X + leftD, topLeft.Y);
                    var topLineEndPoint = new Point(topRight.X - rightD, topRight.Y);

                    var rightLineStartPoint = new Point(topRight.X, topRight.Y - topD);
                    var rightLineEndPoint = new Point(bottomRight.X, bottomRight.Y + bottomD);

                    var bottomLineStartPoint = new Point(bottomRight.X - rightD, bottomRight.Y);
                    var bottomLineEndPoint = new Point(bottomLeft.X + leftD, bottomLeft.Y);

                    var leftGuidelines = new GuidelineSet();
                    leftGuidelines.GuidelinesX.Add(leftLineStartPoint.X - leftD);
                    leftGuidelines.GuidelinesX.Add(leftLineStartPoint.X + leftD);
                    leftGuidelines.GuidelinesY.Add(leftLineStartPoint.Y);
                    leftGuidelines.GuidelinesY.Add(leftLineEndPoint.Y);

                    var topGuidelines = new GuidelineSet();
                    topGuidelines.GuidelinesX.Add(topLineStartPoint.X);
                    topGuidelines.GuidelinesX.Add(topLineEndPoint.X);
                    topGuidelines.GuidelinesY.Add(topLineStartPoint.Y - topD);
                    topGuidelines.GuidelinesY.Add(topLineStartPoint.Y + topD);

                    var rightGuidelines = new GuidelineSet();
                    rightGuidelines.GuidelinesX.Add(rightLineStartPoint.X - rightD);
                    rightGuidelines.GuidelinesX.Add(rightLineStartPoint.X + rightD);
                    rightGuidelines.GuidelinesY.Add(rightLineStartPoint.Y);
                    rightGuidelines.GuidelinesY.Add(rightLineEndPoint.Y);

                    var bottomGuidelines = new GuidelineSet();
                    bottomGuidelines.GuidelinesX.Add(bottomLineStartPoint.X);
                    bottomGuidelines.GuidelinesX.Add(bottomLineEndPoint.X);
                    bottomGuidelines.GuidelinesY.Add(bottomLineStartPoint.Y - bottomD);
                    bottomGuidelines.GuidelinesY.Add(bottomLineStartPoint.Y + bottomD);

                    dc.PushGuidelineSet(leftGuidelines);
                    dc.DrawLine(leftPen, leftLineStartPoint, leftLineEndPoint);
                    dc.Pop();

                    dc.PushGuidelineSet(topGuidelines);
                    dc.DrawLine(topPen, topLineStartPoint, topLineEndPoint);
                    dc.Pop();

                    dc.PushGuidelineSet(rightGuidelines);
                    dc.DrawLine(rightPen, rightLineStartPoint, rightLineEndPoint);
                    dc.Pop();

                    dc.PushGuidelineSet(bottomGuidelines);
                    dc.DrawLine(bottomPen, bottomLineStartPoint, bottomLineEndPoint);
                    dc.Pop();

                    if (!string.IsNullOrEmpty(roi.Title))
                    {
                        var titleText = new FormattedText(roi.Title, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                            new Typeface("宋体"), emSize, color, VisualTreeHelper.GetDpi(this).PixelsPerDip);

                        var titlePoint = new Point(x, y + h + 10);
                        dc.DrawText(titleText, titlePoint);
                    }

                    if (roi.ShowRoiValue)
                    {
                        var roundX = Math.Round(roi.X);
                        var roundY = Math.Round(roi.Y);
                        var roundWidth = Math.Round(roi.Width);
                        var roundHeight = Math.Round(roi.Height);

                        var xyValues = $"X:{roundX} Y:{roundY}, W:{roundWidth} H:{roundHeight}";

                        var xyText = new FormattedText(xyValues, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                            new Typeface("宋体"), emSize, color, VisualTreeHelper.GetDpi(this).PixelsPerDip);

                        var xyStartPoint = new Point(x, y + h + 10);

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
        private readonly Pen _editorPen = new Pen(Brushes.Gray, 1);
        private readonly Pen _clearPen = new Pen(Brushes.Transparent, 1);
        private readonly float _physicPixel;
        public RoiEditorDrawingVisual()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

            var dpiX = (int)dpiXProperty.GetValue(null, null);
            var dpiY = (int)dpiYProperty.GetValue(null, null);

            var pixelsPerDpi = (float)dpiX / 96;
            _physicPixel = 1 / pixelsPerDpi;
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

                if (thickness % _physicPixel != 0)
                {
                    var intScale = Math.Round(thickness / _physicPixel);
                    thickness = intScale * _physicPixel;
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
                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(bottomLeft.X - radius / 2, bottomLeft.Y - radius / 2, radius, radius));
                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(bottomCenter.X - radius / 2, bottomCenter.Y - radius / 2, radius, radius));
                dc.DrawRectangle(Brushes.Transparent, _editorPen, new Rect(bottomRight.X - radius / 2, bottomRight.Y - radius / 2, radius, radius));
            }
        }
        public void ClearEditor()
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                dc.DrawRectangle(Brushes.Transparent, _clearPen,
                    new Rect(0, 0, 1, 1));
            }
        }
    }

    public class RoiCreatorDrawingVisual : DrawingVisual
    {
        private readonly BrushConverter _brushConverter = new BrushConverter();
        private readonly Pen _clearPen = new Pen(Brushes.Transparent, 1);
        private readonly float _physicPixel;

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }

        public RoiCreatorDrawingVisual()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

            var dpiX = (int)dpiXProperty.GetValue(null, null);
            var dpiY = (int)dpiYProperty.GetValue(null, null);

            var pixelsPerDpi = (float)dpiX / 96;
            _physicPixel = 1 / pixelsPerDpi;
        }

        public void DrawCreator(double x, double y, double width, double height, string stringColor, float thickness)
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

                    double penThickness = thickness;
                    if (penThickness % _physicPixel != 0)
                    {
                        var intScale = Math.Round(penThickness / _physicPixel);
                        penThickness = intScale * _physicPixel;
                    }

                    if (X % _physicPixel != 0)
                    {
                        var intScale = Math.Round(X / _physicPixel);
                        X = intScale * _physicPixel;
                    }
                    if (Y % _physicPixel != 0)
                    {
                        var intScale = Math.Round(Y / _physicPixel);
                        Y = intScale * _physicPixel;
                    }
                    if (Width % _physicPixel != 0)
                    {
                        var intScale = Math.Round(Width / _physicPixel);
                        Width = intScale * _physicPixel;
                    }
                    if (Height % _physicPixel != 0)
                    {
                        var intScale = Math.Round(Height / _physicPixel);
                        Height = intScale * _physicPixel;
                    }

                    var color = (SolidColorBrush)_brushConverter.ConvertFromString(stringColor);
                    var pen = new Pen(color, penThickness);
                    var topLeft = new Point(X, Y);
                    var bottomRight = new Point(X + Width, Y + Height);

                    var d = pen.Thickness / 2;
                    var guidelines = new GuidelineSet(new[]
                    {
                        X - d, X + d, X + Width - d, X + Width + d
                    }, new[]
                    {
                        Y - d, Y + d, Y + Height - d, Y + Height + d
                    });
                    dc.PushGuidelineSet(guidelines);
                    dc.DrawRectangle(Brushes.Transparent, pen,
                        new Rect(topLeft, bottomRight));
                    dc.Pop();
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
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }
    }
}
