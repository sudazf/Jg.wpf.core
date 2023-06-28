using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Jg.wpf.core.Extensions.Types.RoiTypes;

namespace Jg.wpf.controls.Customer.CustomImage
{
    public partial class RoiImage : Image
    {
        private readonly Dictionary<Roi, RoiDrawingVisual> _drawers = new Dictionary<Roi, RoiDrawingVisual>();

        public static readonly DependencyProperty RoiSetProperty =
            DependencyProperty.Register("RoiSet", typeof(List<Roi>), typeof(RoiImage),
                new FrameworkPropertyMetadata(new List<Roi>(),
                    OnRoisPropertyChanged));
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(RoiImage),
                new PropertyMetadata(1d, OnScalePropertyChanged));

        public List<Roi> RoiSet
        {
            get => (List<Roi>)GetValue(RoiSetProperty);
            set => SetValue(RoiSetProperty, value);
        }
        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        private static void OnScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = (RoiImage)d;

            if ((double)e.NewValue == 0)
            {
                return;
            }

            if (image.RoiSet == null)
            {
                return;
            }

            foreach (var roi in image.RoiSet)
            {
                if (image.ActualHeight * image.ActualWidth != 0)
                {
                    if (roi.Show)
                    {
                        if (image._drawers.ContainsKey(roi))
                        {
                            image._drawers[roi].DrawRoi(roi, image.Scale);
                        }
                    }
                }
            }

            if (image._hitRoi != null)
            {
                image._editorDrawingVisual.DrawEditor(image._hitRoi, image.Scale);
            }
        }
        private static void OnRoisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = (RoiImage)d;

            //remove old RoiSet
            if (image.RoiSet != null)
            {
                foreach (var roi in image.RoiSet)
                {
                    roi.OnRoiChanged -= image.OnRoiChanged;
                }

                foreach (var visual in image._drawers.Values)
                {
                    image.RemoveLogicalChild(visual);
                    image.RemoveVisualChild(visual);
                }
                image._drawers.Clear();

                if (image._hitRoi != null)
                {
                    image._editorDrawingVisual.ClearEditor();
                    image._hitRoi = null;
                }
            }

            //Add new RoiSet
            var rois = ((List<Roi>)e.NewValue);
            if (rois == null)
            {
                return;
            }

            foreach (var roi in rois)
            {
                roi.OnRoiChanged += image.OnRoiChanged;

                if (image.ActualHeight * image.ActualWidth != 0)
                {
                    if (roi.Show)
                    {
                        var roiVisual = new RoiDrawingVisual();
                        image.AddLogicalChild(roiVisual);
                        image.AddVisualChild(roiVisual);
                        image._drawers[roi] = roiVisual;

                        roiVisual.DrawRoi(roi, image.Scale);
                    }
                }
            }
        }
        private void OnRoiChanged(object sender, Roi roi)
        {
            //当控件本身宽或高为 0 时，不处理
            if (ActualHeight * ActualWidth != 0)
            {
                if (roi.Show)
                {
                    //should show
                    if (_drawers.ContainsKey(roi))
                    {
                        _drawers[roi].DrawRoi(roi, Scale);

                        if (roi == _hitRoi)
                        {
                            _editorDrawingVisual.DrawEditor(roi, Scale);
                        }
                    }
                    else
                    {
                        var roiVisual = new RoiDrawingVisual();

                        AddLogicalChild(roiVisual);
                        AddVisualChild(roiVisual);

                        _drawers[roi] = roiVisual;

                        //当控件本身宽或高为 0 时，不绘制
                        if (ActualHeight * ActualWidth != 0)
                        {
                            roiVisual.DrawRoi(roi, Scale);
                        }
                    }
                }
                else
                {
                    //don't show
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
    }
}
