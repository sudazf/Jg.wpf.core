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
 
        private static readonly DependencyProperty ScrollExtentWidthProperty = DependencyProperty.Register(
            "ScrollExtentWidth", typeof(double), typeof(ScrollableBehavior), new PropertyMetadata(0.0, ScrollExtentSizePropertyChangedCallback));

        private double ScrollExtentWidth
        {
            get => (double)GetValue(ScrollExtentWidthProperty);
            set => SetValue(ScrollExtentWidthProperty, value);
        }

        private static readonly DependencyProperty ScrollExtentHeightProperty = DependencyProperty.Register(
            "ScrollExtentHeight", typeof(double), typeof(ScrollableBehavior), new PropertyMetadata(0.0, ScrollExtentSizePropertyChangedCallback));

        private double ScrollExtentHeight
        {
            get => (double)GetValue(ScrollExtentHeightProperty);
            set => SetValue(ScrollExtentHeightProperty, value);
        }




        public HorizontalAlignment IndicatorHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(IndicatorHorizontalAlignmentProperty);
            set => SetValue(IndicatorHorizontalAlignmentProperty, value);
        }

        public static readonly DependencyProperty IndicatorHorizontalAlignmentProperty =
            DependencyProperty.Register("IndicatorHorizontalAlignment", typeof(HorizontalAlignment), typeof(ScrollableBehavior), new PropertyMetadata(HorizontalAlignment.Left));


        public VerticalAlignment IndicatorVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(IndicatorVerticalAlignmentProperty);
            set => SetValue(IndicatorVerticalAlignmentProperty, value);
        }

        public static readonly DependencyProperty IndicatorVerticalAlignmentProperty =
            DependencyProperty.Register("IndicatorVerticalAlignment", typeof(VerticalAlignment), typeof(ScrollableBehavior), new PropertyMetadata(VerticalAlignment.Bottom));



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
            var length = 0;
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
