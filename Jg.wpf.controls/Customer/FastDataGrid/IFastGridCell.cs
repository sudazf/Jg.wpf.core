using System.Windows.Media;
using Jg.wpf.core.Extensions.Types;

namespace Jg.wpf.controls.Customer.FastDataGrid
{
    public enum CellDecoration
    {
        None,
        StrikeOutHorizontal,
    }

    public enum TooltipVisibilityMode
    {
        Always,
        OnlyWhenTrimmed,
    }

    public interface IFastGridCell
    {
        Color? BackgroundColor { get; }

        int BlockCount { get; }
        int RightAlignBlockCount { get; }
        IFastGridCellBlock GetBlock(int blockIndex);
        CellDecoration Decoration { get; }
        Color? DecorationColor { get; }

        /// <summary>
        /// return NULL disables inline editor
        /// </summary>
        /// <returns></returns>
        string GetEditText();

        void SetEditText(string value, EditCellType type = EditCellType.TextBox);
        void SetEditText(ICellValue value, EditCellType type = EditCellType.TextBox);
        void SetEditText(JColor value, EditCellType type = EditCellType.ComboBoxColor);
        void SetEditText(string text, JColor value, EditCellType type = EditCellType.SelectableColorText);

        string ToolTipText { get; }
        TooltipVisibilityMode ToolTipVisibility { get; }
    }

    public enum EditCellType
    {
        TextBox,
        ComboBoxOpera,
        ComboBoxColor,
        SelectableColorText
    }
}
