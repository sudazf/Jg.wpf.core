using System.Collections.Generic;
using System.Windows.Media;
using Jg.wpf.core.Extensions.Types;
using Jg.wpf.core.Utility;

namespace Jg.wpf.controls.Customer.FastDataGrid.Controls
{
    public class FastGridBlockImpl : IFastGridCellBlock
    {
        public FastGridBlockType BlockType { get; set; }
        public Color? FontColor { get; set; }
        public bool IsItalic { get; set; }
        public bool IsBold { get; set; }
        public string TextData { get; set; }
        public string ImageSource { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public MouseHoverBehaviours MouseHoverBehaviour { get; set; }
        public object CommandParameter { get; set; }
        public string ToolTip { get; set; }

        public FastGridBlockImpl()
        {
            MouseHoverBehaviour = MouseHoverBehaviours.ShowAllWhenMouseOut;
        }
    }

    public class FastGridCellImpl : IFastGridCell
    {
        public Color? BackgroundColor { get; set; }
        public CellDecoration Decoration { get; set; }
        public Color? DecorationColor { get; set; }


        public string ToolTipText { get; set; }
        public TooltipVisibilityMode ToolTipVisibility { get; set; }

        public List<FastGridBlockImpl> Blocks = new List<FastGridBlockImpl>();

        public int BlockCount
        {
            get { return Blocks.Count; }
        }

        public int RightAlignBlockCount { get; set; }

        public IFastGridCellBlock GetBlock(int blockIndex)
        {
            return Blocks[blockIndex];
        }

        public string GetEditText()
        {
            return null;
        }

        public void SetEditText(string value, EditCellType type = EditCellType.TextBox)
        {
            Blocks[0].TextData = value;
        }
        public void SetEditText(ICellValue value, EditCellType type = EditCellType.TextBox)
        {
            Blocks[0].TextData = value.Display;
        }
        public void SetEditText(JColor value, EditCellType type = EditCellType.TextBox)
        {
            Blocks[0].FontColor = JColorHelper.ToColor(value);
        }
        public void SetEditText(string text, JColor value, EditCellType type = EditCellType.SelectableColorText)
        {
            Blocks[0].TextData = text;
            Blocks[0].FontColor = JColorHelper.ToColor(value);
        }

        public IEnumerable<FastGridBlockImpl> SetBlocks
        {
            set
            {
                Blocks.Clear();
                Blocks.AddRange(value);
            }
        }

        public FastGridBlockImpl AddImageBlock(string image, int width = 16, int height = 16)
        {
            var res = new FastGridBlockImpl
            {
                BlockType = FastGridBlockType.Image,
                ImageWidth = width,
                ImageHeight = height,
                ImageSource = image,
            };
            Blocks.Add(res);
            return res;
        }

        public FastGridBlockImpl AddTextBlock(object text)
        {
            var res = new FastGridBlockImpl
            {
                BlockType = FastGridBlockType.Text,
                TextData = text == null ? null : text.ToString(),
            };
            Blocks.Add(res);
            return res;
        }

        public FastGridBlockImpl AddColorBlock(JColor color)
        {
            var res = new FastGridBlockImpl
            {
                BlockType = FastGridBlockType.Color,
                FontColor = JColorHelper.ToColor(color),
            };
            Blocks.Add(res);
            return res;
        }


    }
}
