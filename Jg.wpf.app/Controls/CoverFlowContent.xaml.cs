using System;
using System.Security.Policy;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// CoverFlowContent.xaml 的交互逻辑
    /// </summary>
    public partial class CoverFlowContent : UserControl
    {
        public CoverFlowContent(Uri image)
        {
            InitializeComponent();

            MyImage.Source = BitmapFrame.Create(image, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
        }
    }
}
