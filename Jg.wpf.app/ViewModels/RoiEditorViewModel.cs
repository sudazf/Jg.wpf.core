using System.Collections.Generic;
using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;
using Jg.wpf.core.Extensions.Types.RoiTypes;

namespace Jg.wpf.app.ViewModels
{
    public class RoiEditorViewModel : ViewModelBase
    {
        private List<Roi> _rois;

        public List<Roi> Rois
        {
            get => _rois;
            set
            {
                if (Equals(value, _rois)) return;
                _rois = value;
                RaisePropertyChanged(nameof(Rois));
            }
        }

        public JCommand ShowRoisCommand { get; }

        public RoiEditorViewModel()
        {
            ShowRoisCommand = new JCommand("ShowRoisCommand", OnShowRois);

        }

        private void OnShowRois(object obj)
        {
            Rois = new List<Roi>
            {
                new Roi(25, 48, 227,185, "Red", title:"1,"),
                new Roi(270, 46, 240,182, "LightGreen",title:"2,"),
                new Roi(26, 475, 486,168, "DeepPink", title:"3,")
            };
        }
    }
}
