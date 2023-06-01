using System.Collections.Generic;
using Jg.wpf.controls.Customer.FastDataGrid.Controls;

namespace Jg.wpf.controls.Customer.FastDataGrid
{
    public interface IFastGridModel
    {
        int ColumnCount { get; }
        int RowCount { get; }
        IFastGridCell GetCell(IFastGridView grid, int row, int column);
        EditCellType GetCellType(int row, int column);
        ICellValue GetCellValue(int row, int column);
        IFastGridCell GetRowHeader(IFastGridView view, int row);
        IFastGridCell GetColumnHeader(IFastGridView view, int column);
        IFastGridCell GetGridHeader(IFastGridView view);
        void AttachView(IFastGridView view);
        void DetachView(IFastGridView view);
        void HandleCommand(IFastGridView view, FastGridCellAddress address, object commandParameter, ref bool handled);

        HashSet<int> GetHiddenColumns(IFastGridView view);
        HashSet<int> GetFrozenColumns(IFastGridView view);
        HashSet<int> GetHiddenRows(IFastGridView view);
        HashSet<int> GetFrozenRows(IFastGridView view);

        void HandleSelectionCommand(IFastGridView view, string command);

        int? SelectedRowCountLimit { get; }
        int? SelectedColumnCountLimit { get; }
    }

    public interface ICellValue
    {
        bool IsReadonly { get; }

        string Display { get; set; }

        object Value { get; set; }

        EditCellType EditCellType { get; }

    }
}
