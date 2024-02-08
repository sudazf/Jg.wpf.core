using System;
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
        private bool _useGlobalRoiThickness;
        private RoiThickness _globalThickness;
        private float _pixelsPerDpi;
        private Roi _selectedRoi;

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
        public bool UseGlobalRoiThickness
        {
            get => _useGlobalRoiThickness;
            set
            {
                if (value == _useGlobalRoiThickness) return;
                _useGlobalRoiThickness = value;
                RaisePropertyChanged(nameof(UseGlobalRoiThickness));
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
        public RoiThickness GlobalThickness
        {
            get => _globalThickness;
            set
            {
                if (Equals(value, _globalThickness)) return;
                _globalThickness = value;
                RaisePropertyChanged(nameof(GlobalThickness));
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

        public Roi SelectedRoi
        {
            get => _selectedRoi;
            set
            {
                if (Equals(value, _selectedRoi)) return;
                _selectedRoi = value;
                RaisePropertyChanged(nameof(SelectedRoi));
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
        public JCommand DeleteRoiCommand { get; }
        
        public RoiEditorViewModel()
        {
            _zoom = 1;
            _rois = new MyObservableCollection<Roi>();

            _globalThickness = new RoiThickness(1);
            _globalThickness.OnSingleThicknessChanged += OnSingleThicknessChanged;

            _useGlobalRoiThickness = true;

            ShowRoisCommand = new JCommand("ShowRoisCommand", OnShowRois);
            IncreaseZoomCommand = new JCommand("IncreaseZoomCommand", OnIncreaseZoom);
            DecreaseZoomCommand = new JCommand("DecreaseZoomCommand", OnDecreaseZoom);
            IncreaseAngleCommand = new JCommand("IncreaseAngleCommand", OnIncreaseAngle);
            DecreaseAngleCommand = new JCommand("DecreaseAngleCommand", OnDecreaseAngle);
            ClearRoisCommand = new JCommand("ClearRoisCommand", OnClearAllRoi);
            DeleteRoiCommand = new JCommand("DeleteRoiCommand", OnDeleteRoi);
        }

        public void SetDpiScale(float pixelsPerDpi)
        {
            _pixelsPerDpi = pixelsPerDpi;
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
            Rois.ClearEx();

            var roi1 = new Roi(25, 48, 227, 185, "Yellow", title: "Title1,");
            var roi2 = new Roi(270, 46, 240, 182, "LightGreen", title: "Title2,", restrictedType: RoiRestrictedTypes.X);
            var roi3 = new Roi(30, 415, 486, 140, "DeepPink", title: "Title3,", restrictedType: RoiRestrictedTypes.Y);

            Rois.Add(roi1);
            Rois.Add(roi2);
            Rois.Add(roi3);
        }
        private void OnClearAllRoi(object obj)
        {
            Rois.ClearEx();
        }
        private void OnDeleteRoi(object obj)
        {
            SelectedRoi.Show = false;
        }

        private void OnSingleThicknessChanged(object sender, RoiThickness e)
        {
            RaisePropertyChanged(nameof(GlobalThickness));
        }
    }
}
