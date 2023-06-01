using Jg.wpf.app.Models;
using Jg.wpf.app.ViewModels;
using Jg.wpf.controls.Customer.FastDataGrid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// FastDataGridDemo.xaml 的交互逻辑
    /// </summary>
    public partial class FastDataGridDemo : UserControl
    {
        private readonly CustomDemo _model;

        private int? _rightClickRow;
        private int? _rightClickColumn;
        public FastDataGridDemo()
        {
            InitializeComponent();

            _model = new CustomDemo();
            grid1.Model = _model;

            grid2.MouseLeftButtonDown += grid2_PreviewMouseLeftButtonDown;
        }

        #region Drag drop demo.
        private void grid2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is FastDataGridViewModel vm)
            {
                var dragDropEffects = DragDropEffects.Copy;
                var dataObj = new DataObject(typeof(TestItem), vm.SelectedTestItem);
                DragDrop.DoDragDrop(grid2, dataObj, dragDropEffects);
            }
        }

        private void grid1_PreviewsDropChanged(object sender, DropEventArgs e)
        {
            if (e.DragEventArgs.Data.GetDataPresent(typeof(TestItem)))
            {
                var data = e.DragEventArgs.Data.GetData(typeof(TestItem));
                var row = e.ActiveRow;
                var col = e.ActiveColumn;

                if (data is TestItem item && row != null && col != null)
                {
                    if (col.Value != 0 && col.Value % 2 != 0)
                    {
                        var value = $"{item.Column1}-{item.Column2}-{item.Column3}";
                        _model.SetCellText(row.Value, col.Value, value);
                        _model.InvalidateCell(row.Value, col.Value);
                    }
                }
            }
        }

        #endregion

        #region Right click ContextMenu demo.
        private void grid1_RightClickChanged(object sender, RightClickEventArgs e)
        {
            _rightClickRow = e.ActiveRow;
            _rightClickColumn = e.ActiveColumn;

            if (e.IsRowHeader)
            {
                grid1.ContextMenu = Resources["menu1"] as ContextMenu;
            }
            else if (e.IsColumnHeader)
            {
                grid1.ContextMenu = Resources["menu2"] as ContextMenu;
            }
            else if (e.ActiveRow != null && e.ActiveColumn != null)
            {
                grid1.ContextMenu = Resources["menu3"] as ContextMenu;
            }
        }

        private void TestMenu1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You click row header menu1.");
        }

        private void TestMenu21_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You click column header menu21.");
        }

        private void TestMenu22_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You click column header menu22.");
        }

        private void TestMenu33_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"You click menu33, Row: {_rightClickRow}, Column: {_rightClickColumn}");
        }

        private void TestMenu32_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"You click menu32, Row: {_rightClickRow}, Column: {_rightClickColumn}");
        }

        private void TestMenu31_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"You click menu31, Row: {_rightClickRow}, Column: {_rightClickColumn}");
        }

        #endregion

        //Set cell display demo.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _model.SetCellText(0, 0, "123");
            _model.InvalidateCell(0, 0);
        }
    }
}
