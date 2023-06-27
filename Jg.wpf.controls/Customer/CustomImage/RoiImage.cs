using System.Windows.Controls;
using System.Windows.Media;

namespace Jg.wpf.controls.Customer.CustomImage
{
    public partial class RoiImage : Image
    {
        private readonly RoiEditorDrawingVisual _editorDrawingVisual;

        protected override int VisualChildrenCount
        {
            get
            {
                if (RoiSet != null)
                {
                    return 1 + RoiSet.Count;
                }

                return 1;
            }
        }

        public RoiImage()
        {
            this.Focusable = true;
            SizeChanged += OnSizeChanged;

            _editorDrawingVisual = new RoiEditorDrawingVisual();

            this.AddLogicalChild(_editorDrawingVisual);
            this.AddVisualChild(_editorDrawingVisual);
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
            {
                return _editorDrawingVisual;
            }

            if (_drawers.ContainsKey(RoiSet[index - 1]))
            {
                return _drawers[RoiSet[index - 1]];
            }

            return null;
        }
    }
}
