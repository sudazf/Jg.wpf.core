using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Jg.wpf.core.Utility.Animations
{
    public class CornerRadiusAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(nameof(From), typeof(CornerRadius?), typeof(CornerRadiusAnimation));
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(nameof(To), typeof(CornerRadius?), typeof(CornerRadiusAnimation));

        public CornerRadius? From
        {
            get => (CornerRadius?)this.GetValue(CornerRadiusAnimation.FromProperty);
            set => this.SetValue(CornerRadiusAnimation.FromProperty, (object)value);
        }

        public CornerRadius? To
        {
            get => (CornerRadius?)this.GetValue(CornerRadiusAnimation.ToProperty);
            set => this.SetValue(CornerRadiusAnimation.ToProperty, (object)value);
        }

        public override Type TargetPropertyType => typeof(CornerRadius);

        public override object GetCurrentValue(object from, object to, AnimationClock clock) => (object)this.GetCurrentValue((CornerRadius)from, (CornerRadius)to, clock);

        protected override Freezable CreateInstanceCore() => (Freezable)new CornerRadiusAnimation();

        public CornerRadius GetCurrentValue(
          CornerRadius from,
          CornerRadius to,
          AnimationClock clock)
        {
            if (!clock.CurrentProgress.HasValue)
                return from;
            CornerRadius? nullable = this.From;
            from = nullable ?? from;
            nullable = this.To;
            to = nullable ?? to;
            double topLeft = from.TopLeft + (to.TopLeft - from.TopLeft) * clock.CurrentProgress.Value;
            double topRight1 = from.TopRight;
            double num1 = to.TopRight - from.TopRight;
            double? currentProgress = clock.CurrentProgress;
            double num2 = currentProgress.Value;
            double num3 = num1 * num2;
            double topRight2 = topRight1 + num3;
            double bottomRight1 = from.BottomRight;
            double num4 = to.BottomRight - from.BottomRight;
            currentProgress = clock.CurrentProgress;
            double num5 = currentProgress.Value;
            double num6 = num4 * num5;
            double bottomRight2 = bottomRight1 + num6;
            double bottomLeft1 = from.BottomLeft;
            double num7 = to.BottomLeft - from.BottomLeft;
            currentProgress = clock.CurrentProgress;
            double num8 = currentProgress.Value;
            double num9 = num7 * num8;
            double bottomLeft2 = bottomLeft1 + num9;
            return new CornerRadius(topLeft, topRight2, bottomRight2, bottomLeft2);
        }
    }
}
