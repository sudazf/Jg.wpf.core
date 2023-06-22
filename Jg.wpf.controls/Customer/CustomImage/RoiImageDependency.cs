using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

        public List<Roi> RoiSet
        {
            get => (List<Roi>)GetValue(RoiSetProperty);
            set => SetValue(RoiSetProperty, value);
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
                    image._editorDrawingVisual.ClearEditor(image._hitRoi);
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

                if (roi.Show)
                {
                    var roiVisual = new RoiDrawingVisual();
                    image.AddLogicalChild(roiVisual);
                    image.AddVisualChild(roiVisual);
                    image._drawers[roi] = roiVisual;
                    roiVisual.DrawRoi(roi);
                }
            }
        }

        private void OnRoiChanged(object sender, Roi roi)
        {
            if (roi.Show)
            {
                //should show
                if (_drawers.ContainsKey(roi))
                {
                    _drawers[roi].DrawRoi(roi);
                    _editorDrawingVisual.DrawEditor(roi);
                }
                else
                {
                    var roiVisual = new RoiDrawingVisual();

                    AddLogicalChild(roiVisual);
                    AddVisualChild(roiVisual);

                    _drawers[roi] = roiVisual;
                    roiVisual.DrawRoi(roi);
                }
            }
            else
            {
                //don't show
                if (_drawers.ContainsKey(roi))
                {
                    RemoveLogicalChild(_drawers[roi]);
                    RemoveVisualChild(_drawers[roi]);

                    if (roi == _hitRoi)
                    {
                        _editorDrawingVisual.ClearEditor(roi);
                    }
                    _drawers.Remove(roi);
                }
            }
        }
    }
}
