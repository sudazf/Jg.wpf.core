using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Jg.wpf.core.Command;
using Jg.wpf.core.Extensions.Collections;
using Jg.wpf.core.Notify;
using Jg.wpf.core.Extensions.Types.RoiTypes;

namespace Jg.wpf.app.ViewModels
{
    public class RoiEditorViewModel : ViewModelBase
    {
        private MyObservableCollection<Roi> _rois;
        private double _zoom;
        private double _angle;
        private bool _allowOverLaid = true;
        private bool _canUseRoiCreator = true;
        private int _maxRoi = 10;

        public bool AllowOverLaid
        {
            get => _allowOverLaid;
            set
            {
                if (value == _allowOverLaid) return;
                _allowOverLaid = value;
                RaisePropertyChanged(nameof(AllowOverLaid));
            }
        }

        public bool CanUseRoiCreator
        {
            get => _canUseRoiCreator;
            set
            {
                if (value == _canUseRoiCreator) return;
                _canUseRoiCreator = value;
                RaisePropertyChanged(nameof(CanUseRoiCreator));
            }
        }

        public MyObservableCollection<Roi> Rois
        {
            get => _rois;
            set
            {
                if (Equals(value, _rois)) return;
                _rois = value;
                RaisePropertyChanged(nameof(Rois));
            }
        }

        public int MaxRoi
        {
            get => _maxRoi;
            set
            {
                if (value == _maxRoi) return;
                _maxRoi = value;
                RaisePropertyChanged(nameof(MaxRoi));
            }
        }


        public double Zoom
        {
            get => _zoom;
            set
            {
                if (value.Equals(_zoom)) return;
                _zoom = value;
                RaisePropertyChanged(nameof(Zoom));
            }
        }

        public double Angle
        {
            get => _angle;
            set
            {
                if (value.Equals(_angle)) return;
                _angle = value;
                RaisePropertyChanged(nameof(Angle));
            }
        }

        public JCommand ShowRoisCommand { get; }
        public JCommand ClearRoisCommand { get; }
        public JCommand IncreaseZoomCommand { get; }
        public JCommand DecreaseZoomCommand { get; }
        public JCommand IncreaseAngleCommand { get; }
        public JCommand DecreaseAngleCommand { get; }

        public RoiEditorViewModel()
        {
            _zoom = 1;
            _rois = new MyObservableCollection<Roi>();
            _rois.ClearInvokeAction = ClearInvokeAction;

            ShowRoisCommand = new JCommand("ShowRoisCommand", OnShowRois);
            IncreaseZoomCommand = new JCommand("IncreaseZoomCommand", OnIncreaseZoom);
            DecreaseZoomCommand = new JCommand("DecreaseZoomCommand", OnDecreaseZoom);
            IncreaseAngleCommand = new JCommand("IncreaseAngleCommand", OnIncreaseAngle);
            DecreaseAngleCommand = new JCommand("DecreaseAngleCommand", OnDecreaseAngle);
            ClearRoisCommand = new JCommand("ClearRoisCommand", OnClearAllRoi);
        }

        private void ClearInvokeAction(MyObservableCollection<Roi> rois)
        {
            
        }

        public void AddRoi(Roi newRoi)
        {
            Rois.Add(newRoi);
        }

        public void RemoveRoi(Roi oldRoi)
        {
            var exist = Rois.FirstOrDefault(r => r.X == oldRoi.X && r.Y == oldRoi.Y &&
                                                 r.Width == oldRoi.Width && r.Height == oldRoi.Height);

            if (exist != null)
            {
                Rois.Remove(exist);
            }
        }

        private void OnIncreaseAngle(object obj)
        {
            Angle += 5;
        }

        private void OnDecreaseAngle(object obj)
        {
            Angle -= 5;
        }

        private void OnIncreaseZoom(object obj)
        {
            var unformatZoom = _zoom + 0.1;
            if (unformatZoom > 5)
            {
                return;
            }
            Zoom = Math.Round(unformatZoom, 1);
        }

        private void OnDecreaseZoom(object obj)
        {
            var unformatZoom = _zoom - 0.1;
            if (unformatZoom < 0.1)
            {
                //return;
            }
            Zoom = Math.Round(unformatZoom, 1);
        }

        private void OnShowRois(object obj)
        {
            var roi1 = new Roi(25, 48, 227, 185, "Yellow", title: "1,");
            var roi2 = new Roi(270, 46, 240, 182, "LightGreen", title: "2,") { RestrictedType = RoiRestrictedTypes.X };
            var roi3 = new Roi(30, 415, 486, 140, "DeepPink", title: "3,") { RestrictedType = RoiRestrictedTypes.Y };

            Rois.Clear();

            Rois.Add(roi1);
            Rois.Add(roi2);
            Rois.Add(roi3);
        }

        private void OnClearAllRoi(object obj)
        {
            Rois.ClearEx();
        }
    }
}
