﻿using Jg.wpf.core.Extensions.Types;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using Jg.wpf.core.Extensions.Types.RoiTypes;
using Point = System.Windows.Point;

namespace Jg.wpf.controls.Customer.CustomImage
{
    public partial class RoiImage
    {
        private OperateType _operate = OperateType.None;
        private bool _isInCreating;
        private Point _startPoint;
        private Point _endPoint;
        private Point _lastPoint;
        private Roi _hitRoi;

        public event EventHandler OnOverMaxRoi;

        public Roi HitRoi
        {
            get => _hitRoi;
            set
            {
                _hitRoi = value; 
                SelectedRoi = _hitRoi;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _operate = OperateType.None;

            if (_isInCreating && CanUseRoiCreator)
            {
                _creatorDrawingVisual.Clear();
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!CanEditRoi)
            {
                if (Cursor != Cursors.Arrow)
                {
                    this.Cursor = Cursors.Arrow;
                }
                return;
            }

            Point point = e.GetPosition(this);

            if (_isInCreating && CanUseRoiCreator)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    _endPoint = new Point(point.X, point.Y);

                    var creatorWidth = _endPoint.X - _startPoint.X;
                    var creatorHeight = _endPoint.Y - _startPoint.Y;

                    if (creatorWidth > 0 && creatorHeight > 0)
                    {
                        _creatorDrawingVisual.DrawCreator((int)_startPoint.X, (int)_startPoint.Y, (int)creatorWidth, (int)creatorHeight, "Red", 1);
                    }
                }
            }

            if (_hitRoi == null)
            {
                return;
            }

            if (_hitRoi.Show == false)
            {
                return;
            }

            var topLeft = new Point(_hitRoi.X, _hitRoi.Y);
            var bottomRight = new Point(_hitRoi.X + _hitRoi.Width, _hitRoi.Y + _hitRoi.Height);
            var topRight = new Point(bottomRight.X, topLeft.Y);
            var bottomLeft = new Point(topLeft.X, bottomRight.Y);
            var topCenter = new Point((topLeft.X + bottomRight.X) / 2, topLeft.Y);
            var bottomCenter = new Point((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
            var leftCenter = new Point(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
            var rightCenter = new Point(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);

            if (_operate == OperateType.None)
            {
                if (HitPointTest(topLeft, point))
                {
                    this.Cursor = Cursors.SizeNWSE;
                    if (e.LeftButton == MouseButtonState.Pressed)
                        _operate = OperateType.TopLeftDrag;
                }
                else if (HitPointTest(bottomRight, point))
                {
                    this.Cursor = Cursors.SizeNWSE;

                    if (e.LeftButton == MouseButtonState.Pressed)
                        _operate = OperateType.BottomRightDrag;
                }
                else if (HitPointTest(topRight, point))
                {
                    this.Cursor = Cursors.SizeNESW;

                    if (e.LeftButton == MouseButtonState.Pressed)
                        _operate = OperateType.TopRightDrag;
                }
                else if (HitPointTest(bottomLeft, point))
                {
                    this.Cursor = Cursors.SizeNESW;

                    if (e.LeftButton == MouseButtonState.Pressed)
                        _operate = OperateType.BottomLeftDrag;
                }
                else if (HitPointTest(topCenter, point))
                {
                    this.Cursor = Cursors.SizeNS;

                    if (e.LeftButton == MouseButtonState.Pressed)
                        _operate = OperateType.TopCenter;
                }
                else if (HitPointTest(bottomCenter, point))
                {
                    this.Cursor = Cursors.SizeNS;

                    if (e.LeftButton == MouseButtonState.Pressed)
                        _operate = OperateType.BottomCenter;
                }
                else if (HitPointTest(leftCenter, point))
                {
                    this.Cursor = Cursors.SizeWE;

                    if (e.LeftButton == MouseButtonState.Pressed)
                        _operate = OperateType.LeftCenter;
                }
                else if (HitPointTest(rightCenter, point))
                {
                    this.Cursor = Cursors.SizeWE;

                    if (e.LeftButton == MouseButtonState.Pressed)
                        _operate = OperateType.RightCenter;
                }
                else if (HitCenterTest(point))
                {
                    this.Cursor = Cursors.SizeAll;
                    if (e.LeftButton == MouseButtonState.Pressed)
                        _operate = OperateType.CenterDrag;
                }

                else
                    this.Cursor = Cursors.Arrow;
            }

            double x, y, width, height, offSetX, offSetY;
            bool overlaid = false;
            switch (_operate)
            {
                case OperateType.None:
                    break;
                case OperateType.TopLeftDrag:
                    topLeft = CoerceTopLeft(point, this);

                    x = topLeft.X;
                    y = topLeft.Y;
                    offSetX = x - _hitRoi.X;
                    offSetY = y - _hitRoi.Y;

                    width = _hitRoi.Width - offSetX;
                    height = _hitRoi.Height - offSetY;

                    overlaid = OverLaidCheck(x, y, width, height, _hitRoi.CanOverLaid);
                    if (!overlaid)
                    {
                        _hitRoi.Update(x, y, width, height);
                        _editorDrawingVisual.DrawEditor(_hitRoi, Scale);
                    }
                    break;
                case OperateType.TopCenter:
                    point = CoerceTopCenter(point, this);

                    topLeft = new Point(topLeft.X, point.Y);

                    x = topLeft.X;
                    y = topLeft.Y;

                    width = bottomRight.X - topLeft.X;
                    height = bottomRight.Y - topLeft.Y;

                    overlaid = OverLaidCheck(x, y, width, height, _hitRoi.CanOverLaid);
                    if (!overlaid)
                    {
                        _hitRoi.Update(x, y, width, height);
                        _editorDrawingVisual.DrawEditor(_hitRoi, Scale);
                    }
                    break;
                case OperateType.TopRightDrag:
                    point = CoerceTopRight(point, this);

                    topLeft = new Point(topLeft.X, point.Y);
                    bottomRight = new Point(point.X, bottomRight.Y);

                    x = topLeft.X;
                    y = topLeft.Y;

                    width = bottomRight.X - topLeft.X;
                    height = bottomRight.Y - topLeft.Y;

                    overlaid = OverLaidCheck(x, y, width, height, _hitRoi.CanOverLaid);
                    if (!overlaid)
                    {
                        _hitRoi.Update(x, y, width, height);
                        _editorDrawingVisual.DrawEditor(_hitRoi, Scale);
                    }
                    break;
                case OperateType.LeftCenter:
                    point = CoerceLeftCenter(point, this);
                    topLeft = new Point(point.X, topLeft.Y);

                    x = topLeft.X;
                    y = topLeft.Y;

                    width = bottomRight.X - topLeft.X;
                    height = bottomRight.Y - topLeft.Y;

                    overlaid = OverLaidCheck(x, y, width, height, _hitRoi.CanOverLaid);
                    if (!overlaid)
                    {
                        _hitRoi.Update(x, y, width, height);
                        _editorDrawingVisual.DrawEditor(_hitRoi, Scale);
                    }
                    break;
                case OperateType.CenterDrag:
                    double xOffset = (point.X - _lastPoint.X);//右方向为正
                    double yOffset = (point.Y - _lastPoint.Y);//下方向为正

                    if (topLeft.X == 0 && xOffset < 0)//不能往左 xOffset不能小于0
                        break;
                    if (topLeft.Y == 0 && yOffset < 0)//不能往上 yOffset不能小于0
                        break;
                    if (bottomRight.X >= this.ActualWidth && xOffset > 0)// 不能往右  xOffset不能大于0
                        break;
                    if (bottomRight.Y >= this.ActualHeight && yOffset > 0)// 不能往下 yOffset不能大于0
                        break;

                    topLeft = CoerceTopLeft(new Point(topLeft.X + xOffset, topLeft.Y + yOffset), this);
                    bottomRight = CoerceBottomRight(new Point(bottomRight.X + xOffset, bottomRight.Y + yOffset), this);

                    x = topLeft.X;
                    y = topLeft.Y;

                    width = bottomRight.X - topLeft.X;
                    height = bottomRight.Y - topLeft.Y;

                    overlaid = OverLaidCheck(x, y, width, height, _hitRoi.CanOverLaid);
                    if (!overlaid)
                    {
                        _hitRoi.Update(x, y, width, height);
                        _editorDrawingVisual.DrawEditor(_hitRoi, Scale);
                    }
                    break;
                case OperateType.RightCenter:
                    point = CoerceRightCenter(point, this);
                    bottomRight = new Point(point.X, bottomRight.Y);

                    x = topLeft.X;
                    y = topLeft.Y;

                    width = bottomRight.X - topLeft.X;
                    height = bottomRight.Y - topLeft.Y;

                    overlaid = OverLaidCheck(x, y, width, height, _hitRoi.CanOverLaid);
                    if (!overlaid)
                    {
                        _hitRoi.Update(x, y, width, height);
                        _editorDrawingVisual.DrawEditor(_hitRoi, Scale);
                    }
                    break;
                case OperateType.BottomLeftDrag:
                    point = CoerceBottomLeft(point, this);
                    topLeft = new Point(point.X, topLeft.Y);
                    bottomRight = new Point(bottomRight.X, point.Y);

                    x = topLeft.X;
                    y = topLeft.Y;

                    width = bottomRight.X - topLeft.X;
                    height = bottomRight.Y - topLeft.Y;

                    overlaid = OverLaidCheck(x, y, width, height, _hitRoi.CanOverLaid);
                    if (!overlaid)
                    {
                        _hitRoi.Update(x, y, width, height);
                        _editorDrawingVisual.DrawEditor(_hitRoi, Scale);
                    }
                    break;
                case OperateType.BottomCenter:
                    point = CoerceBottomCenter(point, this);
                    bottomRight = new Point(bottomRight.X, point.Y);

                    x = topLeft.X;
                    y = topLeft.Y;

                    width = bottomRight.X - topLeft.X;
                    height = bottomRight.Y - topLeft.Y;

                    overlaid = OverLaidCheck(x, y, width, height, _hitRoi.CanOverLaid);
                    if (!overlaid)
                    {
                        _hitRoi.Update(x, y, width, height);
                        _editorDrawingVisual.DrawEditor(_hitRoi, Scale);
                    }
                    break;
                case OperateType.BottomRightDrag:
                    point = CoerceBottomRight(point, this);
                    x = _hitRoi.X;
                    y = _hitRoi.Y;
                    offSetX = point.X - (_hitRoi.X + _hitRoi.Width);
                    offSetY = point.Y - (_hitRoi.Y + _hitRoi.Height);

                    width = _hitRoi.Width + offSetX;
                    height = _hitRoi.Height + offSetY;

                    overlaid = OverLaidCheck(x, y, width, height, _hitRoi.CanOverLaid);
                    if (!overlaid)
                    {
                        _hitRoi.Update(x, y, width, height);
                        _editorDrawingVisual.DrawEditor(_hitRoi, Scale);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _lastPoint = point;
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (!CanEditRoi)
            {
                return;
            }

            if (_hitRoi == null || !_hitRoi.Show)
            {
                return;
            }

            if (_hitRoi.Width <=0 || _hitRoi.Height <=0)
            {
                return;
            }

            base.OnMouseWheel(e);
            Point point = e.GetPosition(this);

            if (HitCenterTest(point))
            {
                double offset;
                if (e.Delta > 0)
                {
                    offset = -1;
                }
                else
                {
                    offset = 1;
                }

                var topLeft = new Point(_hitRoi.X + offset, _hitRoi.Y + offset);
                var bottomRight = new Point(_hitRoi.X + _hitRoi.Width - offset, _hitRoi.Y + _hitRoi.Height - offset);

                switch (_hitRoi.RestrictedType)
                {
                    case RoiRestrictedTypes.None:
                        if (topLeft.X <= 0 || topLeft.Y <= 0 ||
                            bottomRight.X >= ActualWidth || bottomRight.Y >= ActualHeight)
                        {
                            return;
                        }
                        break;
                    case RoiRestrictedTypes.X:
                        if (topLeft.Y <= 0 || bottomRight.Y >= ActualHeight)
                        {
                            return;
                        }
                        break;
                    case RoiRestrictedTypes.Y:
                        if (topLeft.X <= 0 || bottomRight.X >= ActualWidth)
                        {
                            return;
                        }
                        break;
                }


                var x = topLeft.X;
                var y = topLeft.Y;

                var width = bottomRight.X - topLeft.X;
                var height = bottomRight.Y - topLeft.Y;

                var overlaid = OverLaidCheck(x, y, width, height, _hitRoi.CanOverLaid);
                if (!overlaid)
                {
                    _hitRoi.Update(x, y, width, height);
                    _editorDrawingVisual.DrawEditor(_hitRoi, Scale);
                }
            }

            //prevent ScrollViewer scrolling.
            e.Handled = true;
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.Focus();

            if (!CanEditRoi)
            {
                return;
            }

            _creatorDrawingVisual.Reset();

            _startPoint = e.GetPosition(this);
            _endPoint = e.GetPosition(this);
            _lastPoint = e.GetPosition(this);
            var jPoint = new JPoint((int)_lastPoint.X, (int)_lastPoint.Y);

            var hitRoi = HitRoiTest(jPoint);
            if (hitRoi != null)
            {
                HitRoi = hitRoi;

                if (_hitRoi.Show)
                {
                    _isInCreating = false;

                    _editorDrawingVisual.DrawEditor(hitRoi, Scale);
                }
            }
            else
            {
                if (_hitRoi != null)
                {
                    _editorDrawingVisual.ClearEditor();
                }

                HitRoi = null;
                _isInCreating = true;
            }
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (!CanEditRoi)
            {
                return;
            }

            _operate = OperateType.None;

            if (CanUseRoiCreator && _isInCreating)
            {
                if (_creatorDrawingVisual.Width * _creatorDrawingVisual.Height > 0)
                {
                    _creatorDrawingVisual.Clear();

                    double x, y , width, height;

                    //保持Creator和最终绘制位置一致
                    x = _creatorDrawingVisual.OriginX;
                    y = _creatorDrawingVisual.OriginY;
                    width = _creatorDrawingVisual.OriginWidth;
                    height = _creatorDrawingVisual.OriginHeight;

                    var newRoi = new Roi(x, y, width, height, DefaultRoiColor);
                    var newRect = new Rectangle((int)x, (int)y, (int)width, (int)height);
                    bool overlaid = false;

                    //overlaid check
                    foreach (var roi in RoiSet)
                    {
                        var existRectangle = new Rectangle((int)roi.X, (int)roi.Y, (int)roi.Width, (int)roi.Height);
                        overlaid = !Rectangle.Intersect(newRect, existRectangle).IsEmpty;
                        if (overlaid)
                        {
                            break;
                        }
                    }
                    if (overlaid && !AllowOverLaid)
                    {
                        return;
                    }

                    if (RoiSet.Count >= MaxRoi)
                    {
                        OnOverMaxRoi?.Invoke(this, EventArgs.Empty);
                        return;
                    }

                    RoiSet.Add(newRoi);
                }
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!CanEditRoi)
            {
                e.Handled = false;
                return;
            }

            if (_hitRoi == null)
            {
                return;
            }

            bool overlaid = false;
            switch (e.Key)
            {
                case Key.Up:
                    if (_hitRoi.RestrictedType == RoiRestrictedTypes.Y)
                    {
                        e.Handled = true;
                        return;
                    }
                    if (_hitRoi.Y > 0)
                    {
                        var newY = _hitRoi.Y - 1;
                        overlaid = OverLaidCheck(_hitRoi.X, newY, _hitRoi.Width, _hitRoi.Height, _hitRoi.CanOverLaid);
                        if (!overlaid)
                        {
                            _hitRoi.Y = newY;
                        }
                    }
                    e.Handled = true;
                    break;
                case Key.Down:
                    if (_hitRoi.RestrictedType == RoiRestrictedTypes.Y)
                    {
                        e.Handled = true;
                        return;
                    }
                    if (_hitRoi.Y + _hitRoi.Height < ActualHeight)
                    {
                        var newY = _hitRoi.Y + 1;
                        overlaid = OverLaidCheck(_hitRoi.X, newY, _hitRoi.Width, _hitRoi.Height, _hitRoi.CanOverLaid);
                        if (!overlaid)
                        {
                            _hitRoi.Y = newY;
                        }
                    }
                    e.Handled = true;
                    break;
                case Key.Left:
                    if (_hitRoi.RestrictedType == RoiRestrictedTypes.X)
                    {
                        e.Handled = true;
                        return;
                    }
                    if (_hitRoi.X > 0)
                    {
                        var newX = _hitRoi.X - 1;
                        overlaid = OverLaidCheck(newX, _hitRoi.Y, _hitRoi.Width, _hitRoi.Height, _hitRoi.CanOverLaid);
                        if (!overlaid)
                        {
                            _hitRoi.X = newX;
                        }
                    }
                    e.Handled = true;
                    break;
                case Key.Right:
                    if (_hitRoi.RestrictedType == RoiRestrictedTypes.X)
                    {
                        e.Handled = true;
                        return;
                    }
                    if (_hitRoi.X + _hitRoi.Width < ActualWidth)
                    {
                        var newX = _hitRoi.X + 1;
                        overlaid = OverLaidCheck(newX, _hitRoi.Y, _hitRoi.Width, _hitRoi.Height, _hitRoi.CanOverLaid);
                        if (!overlaid)
                        {
                            _hitRoi.X = newX;
                        }
                    }
                    e.Handled = true;
                    break;
                case Key.Delete:
                    RoiSet.Remove(_hitRoi);
                    this.Cursor = Cursors.Arrow;
                    e.Handled = true;
                    break;
            }
        }

        private bool HitPointTest(Point target, Point point)
        {
            double offset = 8;

            if (point.X > target.X + offset)
                return false;

            if (point.X < target.X - offset)
                return false;

            if (point.Y > target.Y + offset)
                return false;

            if (point.Y < target.Y - offset)
                return false;

            return true;
        }
        private bool HitCenterTest(Point point)
        {
            return _hitRoi.Hit(new JPoint((int)point.X, (int)point.Y));
        }
        private Roi HitRoiTest(JPoint point)
        {
            Roi belong = null;

            if (RoiSet != null)
            {
                foreach (var roi in RoiSet)
                {
                    if (roi.Hit(point))
                    {
                        belong = roi;
                        break;
                    }
                }

            }

            return belong;
        }

        private Point CoerceTopLeft(Point point, RoiImage image)
        {
            if (point.X > image._hitRoi.X + image._hitRoi.Width)
                point.X = image._hitRoi.X + image._hitRoi.Width;

            if (point.Y > image._hitRoi.Y + image._hitRoi.Height)
                point.Y = image._hitRoi.Y + image._hitRoi.Height;

            if (point.X < 0)
                point.X = 0;
            if (point.Y < 0)
                point.Y = 0;

            return point;
        }
        private Point CoerceTopCenter(Point point, RoiImage image)
        {
            if (point.Y > image._hitRoi.Y + image._hitRoi.Height)
                point.Y = image._hitRoi.Y + image._hitRoi.Height;
            if (point.Y < 0)
                point.Y = 0;

            return point;
        }
        private Point CoerceTopRight(Point point, RoiImage image)
        {
            if (point.X < image._hitRoi.X)
            {
                point.X = image._hitRoi.X;
            }
            if (point.Y > image._hitRoi.Y + image._hitRoi.Height)
            {
                point.Y = image._hitRoi.Y + image._hitRoi.Height;
            }

            if (point.X > image.ActualWidth)
                point.X = image.ActualWidth;
            if (point.Y > image.ActualHeight)
                point.Y = image.ActualHeight;

            return point;
        }
        private Point CoerceLeftCenter(Point point, RoiImage image)
        {
            if (point.X > image._hitRoi.X + image._hitRoi.Width)
                point.X = image._hitRoi.X + image._hitRoi.Width;

            if (point.X < 0)
                point.X = 0;
            if (point.Y < 0)
                point.Y = 0;

            return point;
        }
        private Point CoerceRightCenter(Point point, RoiImage image)
        {
            if (point.X < image._hitRoi.X)
                point.X = image._hitRoi.X;

            if (point.X > image.ActualWidth)
                point.X = image.ActualWidth;
            if (point.Y > image.ActualHeight)
                point.Y = image.ActualHeight;

            return point;
        }
        private Point CoerceBottomLeft(Point point, RoiImage image)
        {
            if (point.X > image._hitRoi.X + image._hitRoi.Width)
                point.X = image._hitRoi.X + image._hitRoi.Width;
            if (point.Y < image._hitRoi.Y)
                point.Y = image._hitRoi.Y;

            if (point.X < 0)
                point.X = 0;
            if (point.Y < 0)
                point.Y = 0;

            return point;
        }
        private Point CoerceBottomCenter(Point point, RoiImage image)
        {
            if (point.Y < image._hitRoi.Y)
                point.Y = image._hitRoi.Y;

            if (point.X > image.ActualWidth)
                point.X = image.ActualWidth;
            if (point.Y > image.ActualHeight)
                point.Y = image.ActualHeight;

            return point;
        }
        private Point CoerceBottomRight(Point point, RoiImage image)
        {
            if (point.X < image._hitRoi.X)
            {
                point.X = image._hitRoi.X;
            }

            if (point.Y < image._hitRoi.Y)
            {
                point.Y = image._hitRoi.Y;
            }

            if (point.X > image.ActualWidth)
                point.X = image.ActualWidth;
            if (point.Y > image.ActualHeight)
                point.Y = image.ActualHeight;

            return point;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (RoiSet == null)
            {
                return;
            }

            //当控件本身宽或高为 0 时，不绘制
            if (this.ActualHeight * this.ActualWidth == 0)
            {
                foreach (var roi in RoiSet)
                {
                    if (roi.Show)
                    {
                        if (_drawers.ContainsKey(roi))
                        {
                            RemoveLogicalChild(_drawers[roi]);
                            RemoveVisualChild(_drawers[roi]);
                            _drawers.Remove(roi);

                            if (roi == _hitRoi)
                            {
                                _editorDrawingVisual.ClearEditor();
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var roi in RoiSet)
                {
                    if (roi.Show)
                    {
                        if (!_drawers.ContainsKey(roi))
                        {
                            var roiVisual = new RoiDrawingVisual();
                            AddLogicalChild(roiVisual);
                            AddVisualChild(roiVisual);
                            _drawers[roi] = roiVisual;

                            Thickness thickness;
                            if (UseGlobalRoiThickness)
                            {
                                thickness = new Thickness(GlobalRoiThickness.Left, GlobalRoiThickness.Top, GlobalRoiThickness.Right, GlobalRoiThickness.Bottom);
                            }
                            else
                            {
                                thickness = new Thickness(roi.Thickness.Left, roi.Thickness.Top, roi.Thickness.Right, roi.Thickness.Bottom);
                            }
                            roiVisual.DrawRoi(roi, Scale, thickness);
                        }
                    }
                }
            }
        }

        private void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            //阻止 ScrollViewer 自动滚动，否则选择不到最大 ROI
            e.Handled = true;
        }

        private void OnCustomLoaded(object sender, RoutedEventArgs e)
        {
            ////保证可以ROI可以选择最大像素
            //if (Margin.Left < 1 || Margin.Top < 1 ||
            //    Margin.Right < 1 || Margin.Bottom < 1)
            //{
            //    Margin = new Thickness(4);
            //}

            //目前 只支持 Stretch = None 的情形
            if (Stretch != System.Windows.Media.Stretch.None)
            {
                Stretch = System.Windows.Media.Stretch.None;
            }
        }

        private bool OverLaidCheck(double x, double y, double width, double height, bool allowOverlaid)
        {
            if (allowOverlaid && AllowOverLaid)
            {
                return false;
            }

            bool overlaidResult = false;

            var newRect = new Rectangle((int)x, (int)y, (int)width, (int)height);

            //overlaid check
            int overlaidTimes = 0;
            foreach (var roi in RoiSet)
            {
                var existRectangle = new Rectangle((int)roi.X, (int)roi.Y, (int)roi.Width, (int)roi.Height);
                var overlaid = !Rectangle.Intersect(newRect, existRectangle).IsEmpty;
                if (overlaid)
                {
                    overlaidTimes++;
                }
            }

            if (overlaidTimes > 1)
            {
                //overlaidTimes == 1, ignore self
                overlaidResult = true;
            }

            return overlaidResult;
        }
 
    }
}
