using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Jg.wpf.core.Extensions.Collections;
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
            DependencyProperty.Register("RoiSet", typeof(MyObservableCollection<Roi>), typeof(RoiEditor),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public MyObservableCollection<Roi> RoiSet
        {
            get => (MyObservableCollection<Roi>)GetValue(RoiSetProperty);
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


        public bool AllowOverLaid
        {
            get => (bool)GetValue(AllowOverLaidProperty);
            set => SetValue(AllowOverLaidProperty, value);
        }

        public static readonly DependencyProperty AllowOverLaidProperty =
            DependencyProperty.Register("AllowOverLaid", typeof(bool), 
                typeof(RoiEditor), new PropertyMetadata(true));


        public bool CanUseRoiCreator
        {
            get => (bool)GetValue(CanUseRoiCreatorProperty);
            set => SetValue(CanUseRoiCreatorProperty, value);
        }

        public static readonly DependencyProperty CanUseRoiCreatorProperty =
            DependencyProperty.Register("CanUseRoiCreator", typeof(bool), 
                typeof(RoiEditor), new PropertyMetadata(true));


        public int MaxRoi
        {
            get => (int)GetValue(MaxRoiProperty);
            set => SetValue(MaxRoiProperty, value);
        }

        public static readonly DependencyProperty MaxRoiProperty =
            DependencyProperty.Register("MaxRoi", typeof(int), 
                typeof(RoiEditor), new PropertyMetadata(9999));


        public RoiEditor()
        {
            InitializeComponent();
        }
    }
}
