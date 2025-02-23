using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace Jg.wpf.controls.Behaviors
{
    public class IndicatorScrollBehavior : ScrollableBehavior
    {
        private ContentPresenter _indicator;
        private TranslateTransform _indicatorTranslate;

        public static readonly DependencyProperty IndicatorTemplateProperty = DependencyProperty.RegisterAttached(
            "IndicatorTemplate", typeof(DataTemplate), typeof(IndicatorScrollBehavior), new PropertyMetadata(null));

        public static void SetIndicatorTemplate(DependencyObject element, DataTemplate value)
        {
            element.SetValue(IndicatorTemplateProperty, value);
        }

        public static DataTemplate GetIndicatorTemplate(DependencyObject element)
        {
            return (DataTemplate)element.GetValue(IndicatorTemplateProperty);
        }

        protected override void OnAssociatedObjectLoaded()
        {
            if (Selector != null)
            {
                _indicator = Selector.Template.FindName("PART_Indicator", Selector) as ContentPresenter;
                if (_indicator != null)
                {
                    _indicator.Visibility = Visibility.Collapsed;
                    TransformGroup group = new TransformGroup();
                    _indicator.RenderTransform = group;
                    _indicatorTranslate = new TranslateTransform();
                    group.Children.Add(_indicatorTranslate);
                }
            }
            base.OnAssociatedObjectLoaded();
        }

        protected override void OnSelectionChanged(ContentControl selectedElement, ScrollViewer scrollViewer, Point point)
        {
            if (_indicator != null && _indicatorTranslate != null)
            {
                _indicator.Visibility = Visibility.Visible;

                double lengthFrom, lengthTo, translateFrom, translateTo;
                DependencyProperty lengthProperty, translateProperty;
                switch (GetOrientation(AssociatedObject))
                {
                    case JgOrientation.Horizontal:
                    default:
                        lengthFrom = _indicator.RenderSize.Width;
                        lengthTo = selectedElement.RenderSize.Width;
                        //修复：动画线可能会消失的问题
                        if (selectedElement.Content != null && lengthTo == 0)
                        {
                            lengthTo = lengthFrom;
                        }

                        translateFrom = _indicatorTranslate.X;
                        translateTo = scrollViewer.HorizontalOffset + point.X;
                        lengthProperty = FrameworkElement.WidthProperty;
                        translateProperty = TranslateTransform.XProperty;
                        break;
                    case JgOrientation.Vertical:
                        lengthFrom = _indicator.RenderSize.Height;
                        lengthTo = selectedElement.RenderSize.Height;
                        //修复：动画线可能会消失的问题
                        if (selectedElement.Content != null && lengthTo == 0)
                        {
                            lengthTo = lengthFrom;
                        }
                        translateFrom = _indicatorTranslate.Y;
                        translateTo = scrollViewer.VerticalOffset + point.Y;
                        lengthProperty = FrameworkElement.HeightProperty;
                        translateProperty = TranslateTransform.YProperty;
                        break;
                }

                var lengthAnimation = new DoubleAnimation
                {
                    From = lengthFrom,
                    To = lengthTo,
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                };
                _indicator.BeginAnimation(lengthProperty, lengthAnimation);
                var translateAnimation = new DoubleAnimation
                {
                    From = translateFrom,
                    To = translateTo,
                    AccelerationRatio = .3,
                    DecelerationRatio = .69,
                    Duration = new Duration(TimeSpan.FromMilliseconds(400)),
                };
                _indicatorTranslate.BeginAnimation(translateProperty, translateAnimation);
            }
        }
    }
}
