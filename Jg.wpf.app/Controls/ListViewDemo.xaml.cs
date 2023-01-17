using System.Windows.Controls;
using System.Windows.Data;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// ListViewDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ListViewDemo : UserControl
    {
        public ListViewDemo()
        {
            InitializeComponent();

            this.GridView.Columns.Add(new GridViewColumn()
            {
                DisplayMemberBinding = new Binding("Address"),
                Header = "Address",
                Width = 150,
            });
        }
    }
}
