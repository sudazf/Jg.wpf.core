using System.Windows;
using System.Windows.Controls;
using Jg.wpf.app.ViewModels;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// DraggableDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class DraggableDataGrid : UserControl
    {
        public DraggableDataGrid()
        {
            InitializeComponent();

        }

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGridRow draggedItem)
            {
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        private void DataGridRow_Drop(object sender, System.Windows.DragEventArgs e)
        {
            //var droppedData = e.Data.GetData(typeof(RowItem)) as RowItem;
            //var target = ((DataGridRow)(sender)).DataContext as RowItem;

            //int removedIdx = Data.Items.IndexOf(droppedData);
            //int targetIdx = Data.Items.IndexOf(target);

            //if (removedIdx < targetIdx)
            //{
            //    if (DataContext is DraggableDataGridViewModel vm)
            //    {
            //        vm.Rows.Insert(targetIdx + 1, droppedData);
            //        vm.Rows.RemoveAt(removedIdx);
            //    }
     
            //}
            //else
            //{
            //    int remIdx = removedIdx + 1;
            //    if (DataContext is DraggableDataGridViewModel vm)
            //    {
            //        if (vm.Rows.Count + 1 > remIdx)
            //        {
            //            vm.Rows.Insert(targetIdx, droppedData);
            //            vm.Rows.RemoveAt(remIdx);
            //        }
            //    }
      
            //}
        }
    }
}
