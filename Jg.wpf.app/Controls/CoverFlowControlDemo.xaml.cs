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
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/CoverFlow/CoverFlow-1.png")),
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/CoverFlow/CoverFlow-2.png")),
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/CoverFlow/CoverFlow-3.png")),
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/CoverFlow/CoverFlow-4.png")),
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/CoverFlow/CoverFlow-5.png")),
                new CoverFlowContent(new Uri(@"pack://application:,,,/Images/CoverFlow/CoverFlow-6.png")),
            });

            CoverFlowMain.PageIndex = 2;
        }
    }
}
