﻿using System.Collections.Generic;
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
                new Roi(85, 70, 105,110, "Red"),
                new Roi(260, 220, 100,100, "LightGreen"),
                new Roi(290, 30, 160,110, "DeepPink")
            };
        }
    }
}