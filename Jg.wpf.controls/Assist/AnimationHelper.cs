using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Jg.wpf.core.Utility;

namespace Jg.wpf.controls.Assist
{
    public class AnimationHelper
    {
        private static DoubleAnimation RotateAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(200)));

        public static readonly DependencyProperty AllowsAnimationProperty = DependencyProperty.RegisterAttached("AllowsAnimation"
            , typeof(bool), typeof(AnimationHelper), new FrameworkPropertyMetadata(false, AllowsAnimationChanged));

        public static bool GetAllowsAnimation(DependencyObject d)
        {
            return (bool)d.GetValue(AllowsAnimationProperty);
        }

        public static void SetAllowsAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowsAnimationProperty, value);
        }

        private static void AllowsAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as FrameworkElement;
            if (uc == null) return;
            if (uc.RenderTransformOrigin == new Point(0, 0))
            {
                uc.RenderTransformOrigin = new Point(0.5, 0.5);
                RotateTransform trans = new RotateTransform(0);
                uc.RenderTransform = trans;
            }
            var value = (bool)e.NewValue;
            if (value)
            {
                RotateAnimation.To = 180;
                uc.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, RotateAnimation);
            }
            else
            {
                RotateAnimation.To = 0;
                uc.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, RotateAnimation);
            }
        }


        /// <summary>
        ///     创建一个Thickness动画
        /// </summary>
        /// <param name="thickness"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static ThicknessAnimation CreateAnimation(Thickness thickness = default, double milliseconds = 200)
        {
            return new(thickness, new Duration(TimeSpan.FromMilliseconds(milliseconds)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };
        }

        /// <summary>
        ///     创建一个Double动画
        /// </summary>
        /// <param name="toValue"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DoubleAnimation CreateAnimation(double toValue, double milliseconds = 200)
        {
            return new(toValue, new Duration(TimeSpan.FromMilliseconds(milliseconds)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };
        }

        internal static void DecomposeGeometryStr(string geometryStr, out double[] arr)
        {
            var collection = Regex.Matches(geometryStr, RegexPatterns.DigitsPattern);
            arr = new double[collection.Count];
            for (var i = 0; i < collection.Count; i++)
            {
                arr[i] = collection[i].Value.Value<double>();
            }
        }

        internal static Geometry ComposeGeometry(string[] strings, double[] arr)
        {
            var builder = new StringBuilder(strings[0]);
            for (var i = 0; i < arr.Length; i++)
            {
                var s = strings[i + 1];
                var n = arr[i];
                if (!double.IsNaN(n))
                {
                    builder.Append(n).Append(s);
                }
            }

            return Geometry.Parse(builder.ToString());
        }

        internal static Geometry InterpolateGeometry(double[] from, double[] to, double progress, string[] strings)
        {
            var accumulated = new double[to.Length];
            for (var i = 0; i < to.Length; i++)
            {
                var fromValue = from[i];
                accumulated[i] = fromValue + (to[i] - fromValue) * progress;
            }

            return ComposeGeometry(strings, accumulated);
        }

        internal static double[] InterpolateGeometryValue(double[] from, double[] to, double progress)
        {
            var accumulated = new double[to.Length];
            for (var i = 0; i < to.Length; i++)
            {
                var fromValue = from[i];
                accumulated[i] = fromValue + (to[i] - fromValue) * progress;
            }

            return accumulated;
        }
    }
}
