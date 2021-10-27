using System.Collections.ObjectModel;
using Jg.wpf.controls.Customer.LayoutPanel;
using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class DraggableDemoViewModel
    {
        public DraggableViewModel DraggableDataGridViewModel { get; set; }
        public DraggableViewModel DraggableListBoxViewModel { get; set; }
        public DraggableViewModel DraggableItemsControlViewModel { get; set; }

        public DraggableDemoViewModel()
        {

            DraggableDataGridViewModel = new DraggableViewModel();
            DraggableListBoxViewModel = new DraggableViewModel();
            DraggableItemsControlViewModel = new DraggableViewModel();
        }
    }

    public class DraggableViewModel : ViewModelBase
    {
        private bool _isInDraggingModel;
        public bool IsInDraggingMode
        {
            get => _isInDraggingModel;
            set
            {
                _isInDraggingModel = value;
                RaisePropertyChanged(() => IsInDraggingMode);
            }
        }

        public ObservableCollection<RowItem> Rows { get; }
        public JCommand ItemDroppedCommand { get; }

        public DraggableViewModel()
        {
            ItemDroppedCommand = new JCommand("ItemDroppedCommand", OnDropItem, a => true, "ItemDropped");

            Rows = new ObservableCollection<RowItem>()
            {
                new RowItem("Cell Value 11","Cell Value 12","Cell Value 13","Cell Value 14","Cell Value 15"),
                new RowItem("Cell Value 21","Cell Value 2","Cell Value 3","Cell Value 4","Cell Value 5"),
                new RowItem("Cell Value 31","Cell Value 2","Cell Value 3","Cell Value 4","Cell Value 5"),
                new RowItem("Cell Value 41","Cell Value 2","Cell Value 3","Cell Value 4","Cell Value 5"),
                new RowItem("Cell Value 51","Cell Value 2","Cell Value 3","Cell Value 4","Cell Value 5"),
                new RowItem("Cell Value 61","Cell Value 2","Cell Value 3","Cell Value 4","Cell Value 5"),
                new RowItem("Cell Value 71","Cell Value 2","Cell Value 3","Cell Value 4","Cell Value 5"),
                new RowItem("Cell Value 81","Cell Value 2","Cell Value 3","Cell Value 4","Cell Value 5"),
            };
        }

        private void OnDropItem(object obj)
        {
            if (obj is IItemDroppedEventArgs args &&
                args.CurrentIndex >= 0 && args.PreviousIndex >= 0 &&
                args.CurrentIndex != args.PreviousIndex)
            {
                var p = args.PreviousIndex;
                var c = args.CurrentIndex;

                Rows.Move(p, c);
            }
        }
    }

    public class RowItem
    {
        public string CellProperty1 { get; }
        public string CellProperty2 { get; }
        public string CellProperty3 { get; }
        public string CellProperty4 { get; }
        public string CellProperty5 { get; }

        public RowItem(string p1, string p2, string p3, string p4, string p5)
        {
            CellProperty1 = p1;
            CellProperty2 = p2;
            CellProperty3 = p3;
            CellProperty4 = p4;
            CellProperty5 = p5;
        }
    }
}
