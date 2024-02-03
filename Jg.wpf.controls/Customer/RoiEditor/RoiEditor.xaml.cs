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
            DependencyProperty.Register(nameof(RoiSet), typeof(MyObservableCollection<Roi>), typeof(RoiEditor),
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
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(RoiEditor), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register(nameof(Scale), typeof(double), typeof(RoiEditor), 
                new PropertyMetadata(1d));

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(RoiEditor), 
                new PropertyMetadata(0d));


        public bool AllowOverLaid
        {
            get => (bool)GetValue(AllowOverLaidProperty);
            set => SetValue(AllowOverLaidProperty, value);
        }

        public static readonly DependencyProperty AllowOverLaidProperty =
            DependencyProperty.Register(nameof(AllowOverLaid), typeof(bool), 
                typeof(RoiEditor), new PropertyMetadata(true));


        public bool CanUseRoiCreator
        {
            get => (bool)GetValue(CanUseRoiCreatorProperty);
            set => SetValue(CanUseRoiCreatorProperty, value);
        }

        public static readonly DependencyProperty CanUseRoiCreatorProperty =
            DependencyProperty.Register(nameof(CanUseRoiCreator), typeof(bool), 
                typeof(RoiEditor), new PropertyMetadata(true));


        public int MaxRoi
        {
            get => (int)GetValue(MaxRoiProperty);
            set => SetValue(MaxRoiProperty, value);
        }

        public static readonly DependencyProperty MaxRoiProperty =
            DependencyProperty.Register(nameof(MaxRoi), typeof(int), 
                typeof(RoiEditor), new PropertyMetadata(9999));


        public Thickness GlobalRoiThickness
        {
            get => (Thickness)GetValue(GlobalRoiThicknessProperty);
            set => SetValue(GlobalRoiThicknessProperty, value);
        }

        public static readonly DependencyProperty GlobalRoiThicknessProperty =
            DependencyProperty.Register(nameof(GlobalRoiThickness), typeof(Thickness),
                typeof(RoiEditor), new PropertyMetadata(new Thickness(2)));

        public bool UseGlobalRoiThickness
        {
            get => (bool)GetValue(UseGlobalRoiThicknessProperty);
            set => SetValue(UseGlobalRoiThicknessProperty, value);
        }

        public static readonly DependencyProperty UseGlobalRoiThicknessProperty =
            DependencyProperty.Register(nameof(UseGlobalRoiThickness), typeof(bool),
                typeof(RoiEditor), new PropertyMetadata(false));

        public RoiEditor()
        {
            InitializeComponent();
        }
    }
}
