using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            var rois = ((List<Roi>)e.NewValue);
            if (rois == null)
            {
                return;
            }

            foreach (var roi in rois)
            {
                var roiVisual = new RoiDrawingVisual();

                image.AddLogicalChild(roiVisual);
                image.AddVisualChild(roiVisual);

                image._drawers[roi] = roiVisual;

                roi.OnRoiChanged += image.OnRoiChanged;
                roiVisual.DrawRoi(roi);
            }
        }

        private void OnRoiChanged(object sender, Roi roi)
        {
            _drawers[roi].DrawRoi(roi);
            _editorDrawingVisual.DrawEditor(roi);
        }
    }
}
