using System;
using System.Windows.Controls;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// CoverFlowControlDemo.xaml 的交互逻辑
    /// </summary>
    public partial class CoverFlowControlDemo : UserControl
    {
        public CoverFlowControlDemo()
        {
            InitializeComponent();

            CoverFlowMain.AddRange(new[]
            {
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/RoiDemo2.jpg")),
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/RoiDemo2.jpg")),
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/RoiDemo2.jpg")),
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/RoiDemo2.jpg")),
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/RoiDemo2.jpg")),
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/RoiDemo2.jpg")),
            });

            CoverFlowMain.PageIndex = 2;
        }
    }
}
