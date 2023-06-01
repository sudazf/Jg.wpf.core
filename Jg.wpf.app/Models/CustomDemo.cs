using Jg.wpf.controls.Customer.FastDataGrid.Controls;
using Jg.wpf.controls.Customer.FastDataGrid;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using Jg.wpf.core.Extensions.Types;
using Jg.wpf.core.Utility;

namespace Jg.wpf.app.Models
{
    public class CustomDemo : DemoBase
    {
        private readonly Dictionary<Tuple<int, int>, EditCellObject> _cells;
        private readonly int _columnCount = 200;
        private readonly int _rowCount = 1000;

        public override int ColumnCount => _columnCount;
        public override int RowCount => _rowCount;

        public CustomDemo()
        {
            _cells = new Dictionary<Tuple<int, int>, EditCellObject>();

            //generate cells data demo.
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _columnCount; j++)
                {
                    var cellType = EditCellType.TextBox;
                    var value = $"Cond-{i}-{j}";
                    var binName = $"Bin-{i}";
                    var key = Tuple.Create(i, j);
                    var color = new JColor(0, 0, 0);
                    var isReadonly = true;
                    if (j == 0)
                    {
                        cellType = EditCellType.SelectableColorText;
                        var colorTextCell = new EditCellObject(cellType, color, binName, false);
                        _cells[key] = colorTextCell;
                        continue;
                    }

                    if (j % 2 == 0)
                    {
                        cellType = EditCellType.ComboBoxOpera;
                        value = "And";
                        isReadonly = false;
                    }

                    var cell = new EditCellObject(cellType, value, value, isReadonly);
                    _cells[key] = cell;
                }
            }
        }


        public override EditCellType GetCellType(int row, int column)
        {
            var key = Tuple.Create(row, column);

            if (_cells.ContainsKey(key))
            {
                return _cells[key].EditCellType;
            }

            return base.GetCellType(row, column);
        }

        public override ICellValue GetCellValue(int row, int column)
        {
            var key = Tuple.Create(row, column);

            if (_cells.ContainsKey(key))
            {
                return _cells[key];
            }

            return base.GetCellValue(row, column);
        }

        public override string GetCellText(int row, int column)
        {
            var key = Tuple.Create(row, column);

            if (_cells.ContainsKey(key))
            {
                return _cells[key].Display;
            }

            return string.Empty;
        }

        public override void SetCellText(int row, int column, string value, EditCellType type = EditCellType.TextBox)
        {
            var key = Tuple.Create(row, column);
            if (_cells.ContainsKey(key))
            {
                _cells[key].Value = value;
                _cells[key].Display = value;
            }
        }

        public override void SetCellText(int row, int column, JColor value, EditCellType type = EditCellType.ComboBoxColor)
        {
            var key = Tuple.Create(row, column);
            if (_cells.ContainsKey(key))
            {
                _cells[key].Value = value;
            }
        }

        public override void SetCellText(int row, int column, string text, JColor color, EditCellType type = EditCellType.SelectableColorText)
        {
            var key = Tuple.Create(row, column);
            if (_cells.ContainsKey(key))
            {
                _cells[key].Value = color;
                _cells[key].Display = text;
            }
        }

        public override IFastGridCell GetGridHeader(IFastGridView view)
        {
            var impl = new FastGridCellImpl();
            var btn = impl.AddImageBlock(view.IsTransposed ? "/Images/flip_horizontal_small.png" : "/Images/flip_vertical_small.png");
            btn.CommandParameter = FastGridControl.ToggleTransposedCommand;
            impl.AddTextBlock(" Bin ");
            return impl;
        }

        public override void HandleCommand(IFastGridView view, FastGridCellAddress address, object commandParameter, ref bool handled)
        {
            base.HandleCommand(view, address, commandParameter, ref handled);
            if (commandParameter is string) MessageBox.Show(commandParameter.ToString());
        }

        public override int? SelectedRowCountLimit
        {
            get { return 100; }
        }

        public override void HandleSelectionCommand(IFastGridView view, string command)
        {
            MessageBox.Show(command);
        }

        public override Color? FontColor
        {
            get
            {
                if (_requestedRow != null && _requestedColumn != null)
                {
                    var key = Tuple.Create(_requestedRow.Value, _requestedColumn.Value);

                    if (_cells.ContainsKey(key))
                    {
                        if (_cells[key].Value is JColor color)
                        {
                            return JColorHelper.ToColor(color);
                        }
                    }
                }

                return base.FontColor;
            }
        }

        public override bool IsBold
        {
            get
            {
                if (_requestedColumn != null && _requestedRow != null)
                {
                    if (_requestedColumn.Value == 0)
                    {
                        return true;
                    }

                    return false;
                }

                return false;
            }
        }


        //public override IFastGridCell GetCell(IFastGridView view, int row, int column)
        //{
        //    var impl = new FastGridCellImpl();

        //    var key = Tuple.Create(row, column);
        //    if (_cells.ContainsKey(key))
        //    {
        //        if (_cells[key].Value is SolarColor color)
        //        {
        //            impl.AddColorBlock(color);
        //            return impl;
        //        }
        //    }

        //    return base.GetCell(view, row, column);
        //}
    }
}
