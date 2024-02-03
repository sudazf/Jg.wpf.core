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

        public MyObservableCollection<Roi> RoiSet
        {
            get => (MyObservableCollection<Roi>)GetValue(RoiSetProperty);
            set => SetValue(RoiSetProperty, value);
        }

        public static readonly DependencyProperty RoiSetProperty =
            DependencyProperty.Register(nameof(RoiSet), typeof(MyObservableCollection<Roi>), typeof(RoiImage),
                new FrameworkPropertyMetadata(null,
                    OnRoiSetPropertyChanged));

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register(nameof(Scale), typeof(double), typeof(RoiImage),
                new PropertyMetadata(1d, OnScalePropertyChanged));


        public bool CanUseRoiCreator
        {
            get => (bool)GetValue(CanUseRoiCreatorProperty);
            set => SetValue(CanUseRoiCreatorProperty, value);
        }

        public static readonly DependencyProperty CanUseRoiCreatorProperty =
            DependencyProperty.Register(nameof(CanUseRoiCreator), typeof(bool), 
                typeof(RoiImage), new PropertyMetadata(true));

        public bool AllowOverLaid
        {
            get { return (bool)GetValue(AllowOverLaidProperty); }
            set { SetValue(AllowOverLaidProperty, value); }
        }

        public static readonly DependencyProperty AllowOverLaidProperty =
            DependencyProperty.Register(nameof(AllowOverLaid), typeof(bool), 
                typeof(RoiImage), new PropertyMetadata(true));


        public string DefaultRoiColor
        {
            get => (string)GetValue(DefaultRoiColorProperty);
            set => SetValue(DefaultRoiColorProperty, value);
        }

        public static readonly DependencyProperty DefaultRoiColorProperty =
            DependencyProperty.Register(nameof(DefaultRoiColor), typeof(string), 
                typeof(RoiImage), new PropertyMetadata("Red"));

        public int MaxRoi
        {
            get => (int)GetValue(MaxRoiProperty);
            set => SetValue(MaxRoiProperty, value);
        }

        public static readonly DependencyProperty MaxRoiProperty =
            DependencyProperty.Register(nameof(MaxRoi), typeof(int), 
                typeof(RoiImage), new PropertyMetadata(9999));

        public Thickness GlobalRoiThickness
        {
            get => (Thickness)GetValue(GlobalRoiThicknessProperty);
            set => SetValue(GlobalRoiThicknessProperty, value);
        }

        public static readonly DependencyProperty GlobalRoiThicknessProperty =
            DependencyProperty.Register(nameof(GlobalRoiThickness), typeof(Thickness), 
                typeof(RoiImage), new PropertyMetadata(new Thickness(2), OnGlobalRoiThicknessChanged));

        public bool CanEditRoi
        {
            get => (bool)GetValue(CanEditRoiProperty);
            set => SetValue(CanEditRoiProperty, value);
        }

        public static readonly DependencyProperty CanEditRoiProperty =
            DependencyProperty.Register(nameof(CanEditRoi), typeof(bool), 
                typeof(RoiImage), new PropertyMetadata(true, OnCanEditRoiChanged));

        public bool UseGlobalRoiThickness
        {
            get => (bool)GetValue(UseGlobalRoiThicknessProperty);
            set => SetValue(UseGlobalRoiThicknessProperty, value);
        }

        public static readonly DependencyProperty UseGlobalRoiThicknessProperty =
            DependencyProperty.Register(nameof(UseGlobalRoiThickness), typeof(bool), 
                typeof(RoiImage), new PropertyMetadata(false, OnUseGlobalRoiThicknessChanged));


        private static void OnUseGlobalRoiThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RoiImage image)
            {
                if (e.NewValue is bool useGlobalRoiThickness)   
                {
                    if (useGlobalRoiThickness)
                    {
                        foreach (var drawer in image._drawers)
                        {
                            drawer.Value.DrawRoi(drawer.Key, image.Scale, image.GlobalRoiThickness);
                        }
                    }
                    else
                    {
                        foreach (var drawer in image._drawers)
                        {
                            var roi = drawer.Key;
                            var thickness = new Thickness(roi.Thickness.Left, roi.Thickness.Top, roi.Thickness.Right, roi.Thickness.Bottom);
                            drawer.Value.DrawRoi(roi, image.Scale, thickness);
                        }
                    }
                }
            }
        }
        private static void OnGlobalRoiThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RoiImage image)
            {
                if (image.UseGlobalRoiThickness && e.NewValue is Thickness thickness)
                {
                    foreach (var drawer in image._drawers)
                    {
                        drawer.Value.DrawRoi(drawer.Key, image.Scale, thickness);
                    }
                }
            }
        }
        private static void OnCanEditRoiChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RoiImage image)
            {
                if (e.NewValue is bool canEdit)
                {
                    if (!canEdit)
                    {
                        if (image._hitRoi != null)
                        {
                            image._editorDrawingVisual.ClearEditor();
                            image._hitRoi = null;
                        }
                    }
                }
            }
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
                            Thickness thickness;
                            if (image.UseGlobalRoiThickness)
                            {
                                thickness = new Thickness(image.GlobalRoiThickness.Left, image.GlobalRoiThickness.Top, image.GlobalRoiThickness.Right, image.GlobalRoiThickness.Bottom);
                            }
                            else
                            {
                                thickness = new Thickness(roi.Thickness.Left, roi.Thickness.Top, roi.Thickness.Right, roi.Thickness.Bottom);
                            }
                            image._drawers[roi].DrawRoi(roi, image.Scale, thickness);
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
                var oldRoiSet = image.RoiSet;
                var oldRoiVisuals = image._drawers.Values;

                oldRoiSet.ClearInvokeAction -= image.ReleaseEventsBeforeClear;
                oldRoiSet.CollectionChanged -= image.OnRoiCollectionChanged;

                foreach (var oldRoi in oldRoiSet)
                {
                    oldRoi.OnRoiChanged -= image.OnRoiChanged;
                }

                foreach (var oldVisual in oldRoiVisuals)
                {
                    image.RemoveLogicalChild(oldVisual);
                    image.RemoveVisualChild(oldVisual);
                }
                image._drawers.Clear();

                if (image._hitRoi != null)
                {
                    image._editorDrawingVisual.ClearEditor();
                    image._hitRoi = null;
                }
            }

            //Add new RoiSet
            var newRoiSet = ((MyObservableCollection<Roi>)e.NewValue);
            if (newRoiSet == null)
            {
                return;
            }

            newRoiSet.ClearInvokeAction += image.ReleaseEventsBeforeClear;
            newRoiSet.CollectionChanged += image.OnRoiCollectionChanged;

            foreach (var newRoi in newRoiSet)
            {
                newRoi.OnRoiChanged += image.OnRoiChanged;

                if (image.ActualHeight * image.ActualWidth != 0 && newRoi.Show)
                {
                    var roiVisual = new RoiDrawingVisual();
                    image.AddLogicalChild(roiVisual);
                    image.AddVisualChild(roiVisual);
                    image._drawers[newRoi] = roiVisual;

                    Thickness thickness;
                    if (image.UseGlobalRoiThickness)
                    {
                        thickness = new Thickness(image.GlobalRoiThickness.Left, image.GlobalRoiThickness.Top, image.GlobalRoiThickness.Right, image.GlobalRoiThickness.Bottom);
                    }
                    else
                    {
                        thickness = new Thickness(newRoi.Thickness.Left, newRoi.Thickness.Top, newRoi.Thickness.Right, newRoi.Thickness.Bottom);
                    }
                    roiVisual.DrawRoi(newRoi, image.Scale, thickness);
                }
            }
        }
        
        private void ReleaseEventsBeforeClear(MyObservableCollection<Roi> rois)
        {
            foreach (var roi in rois)
            {
                roi.OnRoiChanged -= OnRoiChanged;
                roi.Dispose();
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
                    break;
                case NotifyCollectionChangedAction.Remove:

                    if (e.OldItems != null)
                    {
                        roi = (Roi)e.OldItems[0];
                        roi.OnRoiChanged -= OnRoiChanged;
                        roi.Dispose();

                        if (_drawers.ContainsKey(roi))
                        {
                            RemoveLogicalChild(_drawers[roi]);
                            RemoveVisualChild(_drawers[roi]);
                            _drawers.Remove(roi);

                            if (roi == _hitRoi)
                            {
                                _editorDrawingVisual.ClearEditor();
                                _hitRoi = null;
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
                        Thickness thickness;
                        if (UseGlobalRoiThickness)
                        {
                            thickness = new Thickness(GlobalRoiThickness.Left, GlobalRoiThickness.Top, GlobalRoiThickness.Right, GlobalRoiThickness.Bottom);
                        }
                        else
                        {
                            thickness = new Thickness(roi.Thickness.Left, roi.Thickness.Top, roi.Thickness.Right, roi.Thickness.Bottom);
                        }
                        _drawers[roi].DrawRoi(roi, Scale, thickness);

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
                            _hitRoi = null;
                        }
                    }
                }
            }
        }
    }
}
