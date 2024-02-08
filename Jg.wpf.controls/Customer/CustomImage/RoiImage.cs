using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Jg.wpf.controls.Customer.CustomImage
{
    public partial class RoiImage : Image
    {
        private readonly RoiEditorDrawingVisual _editorDrawingVisual;
        private readonly RoiCreatorDrawingVisual _creatorDrawingVisual;
        private readonly float _pixelsPerDpi;

        public RoiImage()
        {
            this.Focusable = true;

            SizeChanged += OnSizeChanged;
            RequestBringIntoView += OnRequestBringIntoView;
            _editorDrawingVisual = new RoiEditorDrawingVisual();
            _creatorDrawingVisual = new RoiCreatorDrawingVisual();

            Loaded += OnCustomLoaded;

            this.AddLogicalChild(_editorDrawingVisual);
            this.AddVisualChild(_editorDrawingVisual);

            AttachCreator();

            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

            var dpiX = (int)dpiXProperty.GetValue(null, null);
            var dpiY = (int)dpiYProperty.GetValue(null, null);

            _pixelsPerDpi = (float)dpiX / 96;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                if (RoiSet == null)
                {
                    return 2;
                }

                return 2 + RoiSet.Count;
            }
        }
        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
            {
                return _editorDrawingVisual;
            }

            if (index == 1)
            {
                return _creatorDrawingVisual;
            }

            if (_drawers.ContainsKey(RoiSet[index - 2]))
            {
                return _drawers[RoiSet[index - 2]];
            }

            return null;
        }

        private void AttachCreator()
        {
            this.AddLogicalChild(_creatorDrawingVisual);
            this.AddVisualChild(_creatorDrawingVisual);
        }
    }
}
