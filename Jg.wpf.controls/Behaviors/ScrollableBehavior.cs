using System;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using Jg.wpf.core.Log;
using Jg.wpf.controls.Customer.JScrollViewer;

namespace Jg.wpf.controls.Behaviors
{
    public class ScrollableBehavior : JgBehavior<JScrollViewer>
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled", typeof(bool), typeof(ScrollableBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetIsEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }


        public static readonly DependencyProperty SelectorProperty = DependencyProperty.Register(
            "Selector", typeof(Selector), typeof(ScrollableBehavior), new PropertyMetadata(null));

        public Selector Selector
        {
            get => (Selector)GetValue(SelectorProperty);
            set => SetValue(SelectorProperty, value);
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.RegisterAttached(
            "Orientation", typeof(JgOrientation), typeof(ScrollableBehavior), new PropertyMetadata(JgOrientation.Horizontal));

        public static void SetOrientation(DependencyObject element, JgOrientation value)
        {
            element.SetValue(OrientationProperty, value);
        }

        public static JgOrientation GetOrientation(DependencyObject element)
        {
            return (JgOrientation)element.GetValue(OrientationProperty);
        }

        public static readonly DependencyProperty MaskLengthProperty = DependencyProperty.RegisterAttached(
            "MaskLength", typeof(double), typeof(ScrollableBehavior), new PropertyMetadata(120.0));

        public static void SetMaskLength(DependencyObject element, double value)
        {
            element.SetValue(MaskLengthProperty, value);
        }

        public static double GetMaskLength(DependencyObject element)
        {
            return (double)element.GetValue(MaskLengthProperty);
        }

        public static readonly DependencyProperty MaskBrushProperty = DependencyProperty.RegisterAttached(
            "MaskBrush", typeof(SolidColorBrush), typeof(ScrollableBehavior), new PropertyMetadata(Brushes.Transparent));

        public static void SetMaskBrush(DependencyObject element, SolidColorBrush value)
        {
            element.SetValue(MaskBrushProperty, value);
        }

        public static SolidColorBrush GetMaskBrush(DependencyObject element)
        {
            return (SolidColorBrush)element.GetValue(MaskBrushProperty);
        }

        public static readonly DependencyPropertyKey StartMaskBackgroundPropertyKey = DependencyProperty.RegisterReadOnly(
            "StartMaskBackground", typeof(Brush), typeof(ScrollableBehavior), new PropertyMetadata(Brushes.Transparent));

        public Brush StartMaskBackground
        {
            get { return (Brush)GetValue(StartMaskBackgroundPropertyKey.DependencyProperty); }
            private set { SetValue(StartMaskBackgroundPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey EndMaskBackgroundPropertyKey = DependencyProperty.RegisterReadOnly(
            "EndMaskBackground", typeof(Brush), typeof(ScrollableBehavior), new PropertyMetadata(Brushes.Transparent));

        public Brush EndMaskBackground
        {
            get { return (Brush)GetValue(EndMaskBackgroundPropertyKey.DependencyProperty); }
            private set { SetValue(EndMaskBackgroundPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey StartMaskOpacityPropertyKey = DependencyProperty.RegisterReadOnly(
            "StartMaskOpacity", typeof(double), typeof(ScrollableBehavior), new PropertyMetadata(0.0));

        public double StartMaskOpacity
        {
            get { return (double)GetValue(StartMaskOpacityPropertyKey.DependencyProperty); }
            private set { SetValue(StartMaskOpacityPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey EndMaskOpacityPropertyKey = DependencyProperty.RegisterReadOnly(
            "EndMaskOpacity", typeof(double), typeof(ScrollableBehavior), new PropertyMetadata(0.0));

        public double EndMaskOpacity
        {
            get { return (double)GetValue(EndMaskOpacityPropertyKey.DependencyProperty); }
            private set { SetValue(EndMaskOpacityPropertyKey, value); }
        }

        private static readonly DependencyProperty ScrollExtentWidthProperty = DependencyProperty.Register(
            "ScrollExtentWidth", typeof(double), typeof(ScrollableBehavior), new PropertyMetadata(0.0, ScrollExtentSizePropertyChangedCallback));

        private double ScrollExtentWidth
        {
            get { return (double)GetValue(ScrollExtentWidthProperty); }
            set { SetValue(ScrollExtentWidthProperty, value); }
        }

        private static readonly DependencyProperty ScrollExtentHeightProperty = DependencyProperty.Register(
            "ScrollExtentHeight", typeof(double), typeof(ScrollableBehavior), new PropertyMetadata(0.0, ScrollExtentSizePropertyChangedCallback));

        private double ScrollExtentHeight
        {
            get { return (double)GetValue(ScrollExtentHeightProperty); }
            set { SetValue(ScrollExtentHeightProperty, value); }
        }

        private static void ScrollExtentSizePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollableBehavior behavior)
            {
                behavior.SelectorOnSelectionChanged(null, null);
            }
        }

        protected override void OnAssociatedObjectLoaded()
        {
            if (Selector != null)
            {
                Selector.SelectionChanged += SelectorOnSelectionChanged;
                SelectorOnSelectionChanged(null, null);
            }
            if (AssociatedObject is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
                ScrollViewerOnScrollChanged(scrollViewer, null);

                Binding widthBinding = new Binding(ScrollViewer.ExtentWidthProperty.Name)
                {
                    Source = scrollViewer
                };
                BindingOperations.SetBinding(this, ScrollExtentWidthProperty, widthBinding);
                Binding heightBinding = new Binding(ScrollViewer.ExtentHeightProperty.Name)
                {
                    Source = scrollViewer
                };
                BindingOperations.SetBinding(this, ScrollExtentHeightProperty, heightBinding);
            }
        }

        protected override void OnCleanUp()
        {
            if (Selector != null)
            {
                Selector.SelectionChanged -= SelectorOnSelectionChanged;
            }
            if (AssociatedObject is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
            }
        }

        private void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                var maskLength = GetMaskLength(scrollViewer);
                var maskColor = GetMaskBrush(scrollViewer).Color;
                Point gradientPoint1, gradientPoint2;
                switch (GetOrientation(AssociatedObject))
                {
                    case JgOrientation.Horizontal:
                    default:
                        gradientPoint1 = new Point(0, 0);
                        gradientPoint2 = new Point(1, 0);
                        UpdateMaskOpacity(scrollViewer.HorizontalOffset, scrollViewer.ExtentWidth, scrollViewer.ViewportWidth, maskLength, maskColor, gradientPoint1, gradientPoint2);
                        break;
                    case JgOrientation.Vertical:
                        gradientPoint1 = new Point(0, 0);
                        gradientPoint2 = new Point(0, 1);
                        UpdateMaskOpacity(scrollViewer.VerticalOffset, scrollViewer.ExtentHeight, scrollViewer.ViewportHeight, maskLength, maskColor, gradientPoint1, gradientPoint2);
                        break;
                }
            }
        }

        private void UpdateMaskOpacity(double offset, double extentLength, double viewportLength, double maskLength, Color maskColor, Point gradientPoint1, Point gradientPoint2)
        {
            var startColor = maskColor;
            var endColor = Color.FromArgb(0, maskColor.R, maskColor.G, maskColor.B);
            LinearGradientBrush startGradientBrush = new LinearGradientBrush();
            startGradientBrush.StartPoint = gradientPoint1;
            startGradientBrush.EndPoint = gradientPoint2;
            startGradientBrush.GradientStops.Add(new GradientStop(startColor, 0));
            startGradientBrush.GradientStops.Add(new GradientStop(endColor, 1));
            StartMaskBackground = startGradientBrush;

            LinearGradientBrush endGradientBrush = new LinearGradientBrush();
            endGradientBrush.StartPoint = gradientPoint2;
            endGradientBrush.EndPoint = gradientPoint1;
            endGradientBrush.GradientStops.Add(new GradientStop(startColor, 0));
            endGradientBrush.GradientStops.Add(new GradientStop(endColor, 1));
            EndMaskBackground = endGradientBrush;

            StartMaskOpacity = Math.Min(maskLength, offset * 3) / maskLength;
            EndMaskOpacity = Math.Min(maskLength, (extentLength - offset - viewportLength) * 3) / maskLength;
        }

        protected virtual void OnSelectionChanged(ContentControl selectedElement, ScrollViewer scrollViewer, Point point)
        {
        }

        private void SelectorOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Selector != null && AssociatedObject is JScrollViewer scrollViewer)
            {
                ContentControl selectedElement = GetSelectedItem(Selector);
                if (selectedElement != null)
                {
                    GeneralTransform focusedVisualTransform = selectedElement.TransformToVisual(scrollViewer);
                    Point point = focusedVisualTransform.Transform(new Point(selectedElement.Margin.Left, selectedElement.Margin.Top));
                    OnSelectionChanged(selectedElement, scrollViewer, point);

                    Rect rect = new Rect(point, selectedElement.RenderSize);
                    switch (GetOrientation(AssociatedObject))
                    {
                        case JgOrientation.Horizontal:
                        default:
                            ScrollAnimation(rect.Left, rect.Right, scrollViewer.HorizontalOffset,
                                scrollViewer.ViewportWidth, scrollViewer.ExtentWidth, scrollViewer,
                                JScrollViewer.BindableHorizontalOffsetProperty);
                            break;
                        case JgOrientation.Vertical:
                            ScrollAnimation(rect.Top, rect.Bottom, scrollViewer.VerticalOffset,
                                scrollViewer.ViewportHeight, scrollViewer.ExtentHeight, scrollViewer,
                                JScrollViewer.BindableVerticalOffsetProperty);
                            break;
                    }
                }
            }
        }

        private void ScrollAnimation(double rectBegin, double rectEnd, double scrollOffset, double scrollLength,
            double scrollExtentLength, ScrollViewer scrollViewer, DependencyProperty offsetProperty)
        {
            var length = GetMaskLength(scrollViewer);
            double begin = rectBegin - length;
            double end = rectEnd + length;
            double offset;
            if (begin < 0)
            {
                offset = scrollOffset + begin;
                if (offset < 0.0)
                {
                    offset = 0.0;
                }

                DoAnimation(scrollViewer, scrollOffset, offset, offsetProperty);
            }
            else if (end > scrollLength)
            {
                offset = scrollOffset + end - scrollLength;
                if (offset + scrollLength > scrollExtentLength)
                {
                    offset = scrollExtentLength - scrollLength;
                }

                DoAnimation(scrollViewer, scrollOffset, offset, offsetProperty);
            }
        }

        private void DoAnimation(ScrollViewer scrollViewer, double scrollOffset, double offset,
            DependencyProperty offsetProperty)
        {
            var doubleAnimation = new DoubleAnimation
            {
                From = scrollOffset,
                To = offset,
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
            };
            scrollViewer.BeginAnimation(offsetProperty, doubleAnimation);
        }

        public ContentControl GetSelectedItem(Selector selector)
        {
            object selectedItem = selector?.SelectedItem;
            if (selectedItem == null)
            {
                return null;
            }

            ContentControl container = selectedItem as ContentControl;
            if (container == null)
            {
                try
                {
                    container = selector.ItemContainerGenerator.ContainerFromIndex(selector.SelectedIndex) as ContentControl;
                    if ((container != null) && !Equals(selectedItem, selector.ItemContainerGenerator.ItemFromContainer(container)))
                    {
                        container = selector.ItemContainerGenerator.ContainerFromItem(selectedItem) as ContentControl;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLineVerbose("GetSelectedItem failed. {0}", e);
                }
            }
            return container;
        }
    }
}
