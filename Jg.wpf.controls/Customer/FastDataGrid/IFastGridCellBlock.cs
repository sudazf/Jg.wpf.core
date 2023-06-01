using System.Windows.Media;

namespace Jg.wpf.controls.Customer.FastDataGrid
{
    public enum FastGridBlockType
    {
        Text,
        Image,
        Color,
        ColorText,
    }

    public enum MouseHoverBehaviours
    {
        HideWhenMouseOut,
        HideButtonWhenMouseOut,
        ShowAllWhenMouseOut,
    }

    public interface IFastGridCellBlock
    {
        FastGridBlockType BlockType { get; }

        Color? FontColor { get; }
        bool IsItalic { get; }
        bool IsBold { get; }
        string TextData { get; }

        string ImageSource { get; }
        int ImageWidth { get; }
        int ImageHeight { get; }

        MouseHoverBehaviours MouseHoverBehaviour { get; }
        object CommandParameter { get; }
        string ToolTip { get; }
    }
}
