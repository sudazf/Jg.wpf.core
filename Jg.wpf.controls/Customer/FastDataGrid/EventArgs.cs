using System;
using Jg.wpf.controls.Customer.FastDataGrid.Controls;

namespace Jg.wpf.controls.Customer.FastDataGrid
{
    public class RowClickEventArgs : EventArgs
    {
        public int Row;
        public FastGridControl Grid;
        public bool Handled;
    }

    public class ColumnClickEventArgs : EventArgs
    {
        public int Column;
        public FastGridControl Grid;
        public bool Handled;
    }
}
