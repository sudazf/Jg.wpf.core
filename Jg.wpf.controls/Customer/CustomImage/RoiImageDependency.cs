using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using Jg.wpf.core.Extensions.Collections;
using Jg.wpf.core.Extensions.Types.RoiTypes;
using Image = System.Windows.Controls.Image;

namespace Jg.wpf.controls.Customer.CustomImage
{
    public partial class RoiImage : Image
    {
        private readonly Dictionary<Roi, RoiDrawingVisual> _drawers = new Dictionary<Roi, RoiDrawingVisual>();

        public static readonly DependencyProperty RoiSetProperty =
            DependencyProperty.Register("RoiSet", typeof(MyObservableCollection<Roi>), typeof(RoiImage),
                new FrameworkPropertyMetadata(null,
                    OnRoiSetPropertyChanged));

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(RoiImage),
                new PropertyMetadata(1d, OnScalePropertyChanged));

        public MyObservableCollection<Roi> RoiSet
        {
            get => (MyObservableCollection<Roi>)GetValue(RoiSetProperty);
            set => SetValue(RoiSetProperty, value);
        }
        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public bool CanUseRoiCreator
        {
            get => (bool)GetValue(CanUseRoiCreatorProperty);
            set => SetValue(CanUseRoiCreatorProperty, value);
        }

        public static readonly DependencyProperty CanUseRoiCreatorProperty =
            DependencyProperty.Register("CanUseRoiCreator", typeof(bool), 
                typeof(RoiImage), new PropertyMetadata(true));

        public bool AllowOverLaid
        {
            get { return (bool)GetValue(AllowOverLaidProperty); }
            set { SetValue(AllowOverLaidProperty, value); }
        }

        public static readonly DependencyProperty AllowOverLaidProperty =
            DependencyProperty.Register("AllowOverLaid", typeof(bool), 
                typeof(RoiImage), new PropertyMetadata(true));


        public string DefaultRoiColor
        {
            get => (string)GetValue(DefaultRoiColorProperty);
            set => SetValue(DefaultRoiColorProperty, value);
        }

        public static readonly DependencyProperty DefaultRoiColorProperty =
            DependencyProperty.Register("DefaultRoiColor", typeof(string), 
                typeof(RoiImage), new PropertyMetadata("Red"));

        public int MaxRoi
        {
            get => (int)GetValue(MaxRoiProperty);
            set => SetValue(MaxRoiProperty, value);
        }

        public static readonly DependencyProperty MaxRoiProperty =
            DependencyProperty.Register("MaxRoi", typeof(int), 
                typeof(RoiImage), new PropertyMetadata(9999));


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
        private static void OnRoiSetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = (RoiImage)d;

            //remove old RoiSet
            if (image.RoiSet != null)
            {
                image.RoiSet.ClearInvokeAction = null;
                image.RoiSet.CollectionChanged -= image.OnRoiCollectionChanged;

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
            var rois = ((MyObservableCollection<Roi>)e.NewValue);
            if (rois == null)
            {
                return;
            }

            rois.ClearInvokeAction = image.ReleaseEventsBeforeClear;
            rois.CollectionChanged += image.OnRoiCollectionChanged;

            foreach (var roi in rois)
            {
                roi.OnRoiChanged += image.OnRoiChanged;

                if (image.ActualHeight * image.ActualWidth != 0 && roi.Show)
                {
                    var roiVisual = new RoiDrawingVisual();
                    image.AddLogicalChild(roiVisual);
                    image.AddVisualChild(roiVisual);
                    image._drawers[roi] = roiVisual;

                    roiVisual.DrawRoi(roi, image.Scale);
                }
            }
        }

        private void ReleaseEventsBeforeClear(MyObservableCollection<Roi> rois)
        {
            foreach (var roi in rois)
            {
                roi.OnRoiChanged -= OnRoiChanged;
            }
        }

        private void OnRoiCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Roi roi;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        roi = (Roi)e.NewItems[0];

                        if (roi.Show)
                        {
                            roi.OnRoiChanged += OnRoiChanged;

                            var roiVisual = new RoiDrawingVisual();
                            AddLogicalChild(roiVisual);
                            AddVisualChild(roiVisual);
                            _drawers[roi] = roiVisual;

                            roiVisual.DrawRoi(roi, Scale);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:

                    if (e.OldItems != null)
                    {
                        roi = (Roi)e.OldItems[0];
                        roi.OnRoiChanged -= OnRoiChanged;

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
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var visual in _drawers.Values)
                    {
                        RemoveLogicalChild(visual);
                        RemoveVisualChild(visual);
                    }

                    _drawers.Clear();

                    if (_hitRoi != null)
                    {
                        _editorDrawingVisual.ClearEditor();
                        _hitRoi = null;
                    }
                    break;
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
