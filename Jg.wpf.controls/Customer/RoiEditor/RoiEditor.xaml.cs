using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Jg.wpf.core.Extensions.Types.RoiTypes;

namespace Jg.wpf.controls.Customer.RoiEditor
{
    /// <summary>
    /// RoiEditor.xaml 的交互逻辑
    /// 使用UserControl而不使用自定义控件的原因是，不想破坏使用者的样式
    /// </summary>
    public partial class RoiEditor : UserControl
    {

        public static readonly DependencyProperty RoiSetProperty =
            DependencyProperty.Register("RoiSet", typeof(List<Roi>), typeof(RoiEditor),
                new FrameworkPropertyMetadata(new List<Roi>(), FrameworkPropertyMetadataOptions.Inherits));

        public List<Roi> RoiSet
        {
            get => (List<Roi>)GetValue(RoiSetProperty);
            set => SetValue(RoiSetProperty, value);
        }

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(RoiEditor), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(RoiEditor), 
                new PropertyMetadata(1d));

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(RoiEditor), 
                new PropertyMetadata(0d));


        public RoiEditor()
        {
            InitializeComponent();
        }

    }
}
