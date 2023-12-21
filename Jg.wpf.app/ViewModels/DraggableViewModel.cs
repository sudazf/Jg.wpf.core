using System.Collections.ObjectModel;
using Jg.wpf.core.Command;
using Jg.wpf.core.Extensions.Types.Animations;
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
            Rows = new ObservableCollection<RowItem>();

            for (int i = 0; i < 20; i++)
            {
                var prefix = $"Cell Value {i}";
                var row = new RowItem($"{prefix}1", $"{prefix}2", $"{prefix}3", $"{prefix}4", $"{prefix}5");
                Rows.Add(row);
            }
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
