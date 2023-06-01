using Jg.wpf.controls.Customer.FastDataGrid.Controls;
using Jg.wpf.controls.Customer.FastDataGrid;
using System.Collections.Generic;
using System.Windows.Media;
using Jg.wpf.core.Extensions.Types;

namespace Jg.wpf.app.Models
{
    public abstract class DemoBase : IFastGridModel, IFastGridCell, IFastGridCellBlock
    {
        private readonly List<IFastGridView> _grids = new List<IFastGridView>();

        private HashSet<int> _frozenRows = new HashSet<int>();
        private HashSet<int> _hiddenRows = new HashSet<int>();
        private HashSet<int> _frozenColumns = new HashSet<int>();
        private HashSet<int> _hiddenColumns = new HashSet<int>();

        protected int? _requestedRow;
        protected int? _requestedColumn;
        public abstract int ColumnCount { get; }

        public abstract int RowCount { get; }

        public virtual string GetCellText(int row, int column)
        {
            return $"Row={row + 1}, Column={column + 1}";
        }

        public virtual void SetCellText(int row, int column, string value, EditCellType type = EditCellType.TextBox)
        {
        }

        public virtual void SetCellText(int row, int column, JColor value, EditCellType type = EditCellType.ComboBoxColor)
        {
        }
        public virtual void SetCellText(int row, int column, string text, JColor value, EditCellType type = EditCellType.SelectableColorText)
        {
        }

        public virtual IFastGridCell GetCell(IFastGridView view, int row, int column)
        {
            _requestedRow = row;
            _requestedColumn = column;
            return this;
        }

        public virtual EditCellType GetCellType(int row, int column)
        {
            return EditCellType.TextBox;
        }

        public virtual ICellValue GetCellValue(int row, int column)
        {
            return null;
        }

        public virtual string GetRowHeaderText(int row)
        {
            return (row + 1).ToString();
        }

        public virtual IFastGridCell GetRowHeader(IFastGridView view, int row)
        {
            _requestedRow = row;
            _requestedColumn = null;
            return this;
        }

        public virtual IFastGridCell GetColumnHeader(IFastGridView view, int column)
        {
            _requestedColumn = column;
            _requestedRow = null;
            return this;
        }

        public virtual IFastGridCell GetGridHeader(IFastGridView view)
        {
            return new FastGridCellImpl();
        }

        public virtual string GetColumnHeaderText(int column)
        {
            if (column == 0)
            {
                return "Bin Name";
            }
            else
            {
                if (column % 2 == 0)
                {
                    return "Opera";
                }
            }

            return $"Cond {column - (column - 1) / 2}";
        }

        public virtual void AttachView(IFastGridView view)
        {
            _grids.Add(view);
        }

        public virtual void DetachView(IFastGridView view)
        {
            _grids.Remove(view);
        }

        public virtual void HandleCommand(IFastGridView view, FastGridCellAddress address, object commandParameter, ref bool handled)
        {
        }

        public virtual HashSet<int> GetHiddenColumns(IFastGridView view)
        {
            return _hiddenColumns;
        }

        public virtual HashSet<int> GetFrozenColumns(IFastGridView view)
        {
            return _frozenColumns;
        }

        public virtual HashSet<int> GetHiddenRows(IFastGridView view)
        {
            return _hiddenRows;
        }

        public virtual HashSet<int> GetFrozenRows(IFastGridView view)
        {
            return _frozenRows;
        }

        public void SetColumnArrange(HashSet<int> hidden, HashSet<int> frozen)
        {
            _hiddenColumns = hidden;
            _frozenColumns = frozen;
            NotifyColumnArrangeChanged();
        }

        public void SetRowArrange(HashSet<int> hidden, HashSet<int> frozen)
        {
            _hiddenRows = hidden;
            _frozenRows = frozen;
            NotifyRowArrangeChanged();
        }

        public void InvalidateAll()
        {
            _grids.ForEach(x => x.InvalidateAll());
        }

        public void InvalidateCell(int row, int column)
        {
            _grids.ForEach(x => x.InvalidateModelCell(row, column));
        }

        public void InvalidateRowHeader(int row)
        {
            _grids.ForEach(x => x.InvalidateModelRowHeader(row));
        }

        public void InvalidateColumnHeader(int column)
        {
            _grids.ForEach(x => x.InvalidateModelColumnHeader(column));
        }

        public void NotifyAddedRows()
        {
            _grids.ForEach(x => x.NotifyAddedRows());
        }

        public void NotifyRefresh()
        {
            _grids.ForEach(x => x.NotifyRefresh());
        }

        public void NotifyColumnArrangeChanged()
        {
            _grids.ForEach(x => x.NotifyColumnArrangeChanged());
        }

        public void NotifyRowArrangeChanged()
        {
            _grids.ForEach(x => x.NotifyRowArrangeChanged());
        }

        public virtual Color? BackgroundColor
        {
            get { return null; }
        }

        public virtual int BlockCount
        {
            get { return 1; }
        }

        public virtual int RightAlignBlockCount
        {
            get { return 0; }
        }

        public virtual IFastGridCellBlock GetBlock(int blockIndex)
        {
            return this;
        }

        public virtual string GetEditText()
        {
            return GetCellText(_requestedRow.Value, _requestedColumn.Value);
        }

        public virtual void SetEditText(string value, EditCellType type = EditCellType.TextBox)
        {
            SetCellText(_requestedRow.Value, _requestedColumn.Value, value, type);
        }

        public void SetEditText(ICellValue value, EditCellType type = EditCellType.TextBox)
        {
            throw new System.NotImplementedException();
        }

        public void SetEditText(JColor value, EditCellType type = EditCellType.ComboBoxColor)
        {
            SetCellText(_requestedRow.Value, _requestedColumn.Value, value, type);
        }

        public void SetEditText(string text, JColor color, EditCellType type = EditCellType.SelectableColorText)
        {
            SetCellText(_requestedRow.Value, _requestedColumn.Value, text, color, type);
        }

        public virtual void HandleSelectionCommand(IFastGridView view, string command)
        {
        }

        public virtual string ToolTipText
        {
            get { return null; }
        }

        public virtual TooltipVisibilityMode ToolTipVisibility
        {
            get { return TooltipVisibilityMode.Always; }
        }

        public virtual FastGridBlockType BlockType
        {
            get
            {
                if (_requestedColumn != null && _requestedRow != null)
                {
                    if (_requestedColumn == 0)
                    {
                        return FastGridBlockType.ColorText;
                    }
                }

                return FastGridBlockType.Text;
            }
        }

        public virtual bool IsItalic
        {
            get { return false; }
        }

        public virtual bool IsBold
        {
            get { return false; }
        }

        public virtual Color? FontColor
        {
            get
            {
                return null;
            }
        }

        public virtual string TextData
        {
            get
            {
                if (_requestedColumn == null && _requestedRow == null) return null;
                if (_requestedColumn != null && _requestedRow != null) return GetCellText(_requestedRow.Value, _requestedColumn.Value);
                if (_requestedColumn != null) return GetColumnHeaderText(_requestedColumn.Value);
                if (_requestedRow != null) return GetRowHeaderText(_requestedRow.Value);
                return null;
            }
        }

        public virtual string ImageSource
        {
            get { return null; }
        }

        public virtual int ImageWidth
        {
            get { return 16; }
        }

        public virtual int ImageHeight
        {
            get { return 16; }
        }

        public virtual MouseHoverBehaviours MouseHoverBehaviour
        {
            get { return MouseHoverBehaviours.ShowAllWhenMouseOut; }
        }

        public virtual object CommandParameter
        {
            get { return null; }
        }

        public virtual string ToolTip
        {
            get { return null; }
        }

        public virtual CellDecoration Decoration
        {
            get { return CellDecoration.None; }
        }

        public virtual Color? DecorationColor
        {
            get { return null; }
        }

        public virtual int? SelectedRowCountLimit
        {
            get { return null; }
        }

        public virtual int? SelectedColumnCountLimit
        {
            get { return null; }
        }
    }
}
