using System;
using System.Windows;

namespace Jg.wpf.controls.Customer.FastDataGrid
{
    public class SelectionChangedEventArgs : EventArgs
    {
        public bool IsInvokedByUser;
        public int? ActiveRow { get; }
        public int? ActiveColumn { get; }

        public SelectionChangedEventArgs(int? row, int? col, bool isInvokedByUser)
        {
            ActiveRow = row;
            ActiveColumn = col;
            IsInvokedByUser = isInvokedByUser;

        }
    }

    public class RightClickEventArgs : EventArgs
    {
        public int? ActiveRow { get; }
        public int? ActiveColumn { get; }
        public bool IsRowHeader { get; }
        public bool IsColumnHeader { get; }

        public RightClickEventArgs(int? row, int? col, bool isRowHeader, bool isColumnHeader)
        {
            ActiveRow = row;
            ActiveColumn = col;
            IsRowHeader = isRowHeader;
            IsColumnHeader = isColumnHeader;
        }
    }

    public class DropEventArgs : EventArgs
    {
        public int? ActiveRow { get; }
        public int? ActiveColumn { get; }
        public bool IsRowHeader { get; }
        public bool IsColumnHeader { get; }
        public DragEventArgs DragEventArgs { get; }

        public DropEventArgs(DragEventArgs e, int? row, int? col, bool isRowHeader, bool isColumnHeader)
        {
            ActiveRow = row;
            ActiveColumn = col;
            IsRowHeader = isRowHeader;
            IsColumnHeader = isColumnHeader;
            DragEventArgs = e;
        }
    }
}
