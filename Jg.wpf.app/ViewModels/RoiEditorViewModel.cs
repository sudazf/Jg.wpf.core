using System.Collections.Generic;
using Jg.wpf.core.Notify;
using Jg.wpf.controls.Customer.CustomImage;

namespace Jg.wpf.app.ViewModels
{
    public class RoiEditorViewModel : ViewModelBase
    {
        public List<Roi> Rois { get; }

        public RoiEditorViewModel()
        {
            Rois = new List<Roi>
            {
                new Roi(85, 70, 105,110, "Red"),
                new Roi(260, 220, 100,100, "LightGreen"),
                new Roi(290, 30, 160,110, "DeepPink")
            };
        }
    }
}
