using System;
using System.Collections.Generic;
using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;
using Jg.wpf.core.Extensions.Types.RoiTypes;

namespace Jg.wpf.app.ViewModels
{
    public class RoiEditorViewModel : ViewModelBase
    {
        private List<Roi> _rois;
        private double _zoom;
        private double _angle;

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
        public JCommand IncreaseZoomCommand { get; }
        public JCommand DecreaseZoomCommand { get; }
        public JCommand IncreaseAngleCommand { get; }
        public JCommand DecreaseAngleCommand { get; }

        public RoiEditorViewModel()
        {
            _zoom = 1;
            _rois = new List<Roi>();

            ShowRoisCommand = new JCommand("ShowRoisCommand", OnShowRois);
            IncreaseZoomCommand = new JCommand("IncreaseZoomCommand", OnIncreaseZoom);
            DecreaseZoomCommand = new JCommand("DecreaseZoomCommand", OnDecreaseZoom);
            IncreaseAngleCommand = new JCommand("IncreaseAngleCommand", OnIncreaseAngle);
            DecreaseAngleCommand = new JCommand("DecreaseAngleCommand", OnDecreaseAngle);
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
            foreach (var roi in Rois)
            {
                roi.OnRoiChanged -= OnRoi1Changed;
                roi.OnRoiChanged -= OnRoi2Changed;
                roi.OnRoiChanged -= OnRoi3Changed;
            }

            var roi1 = new Roi(25, 48, 227, 185, "Yellow", title: "1,");
            var roi2 = new Roi(270, 46, 240, 182, "LightGreen", title: "2,") { RestrictedType = RoiRestrictedTypes.X };
            var roi3 = new Roi(30, 415, 486, 140, "DeepPink", title: "3,") { RestrictedType = RoiRestrictedTypes.Y };

            roi1.OnRoiChanged += OnRoi1Changed;
            roi2.OnRoiChanged += OnRoi2Changed;
            roi3.OnRoiChanged += OnRoi3Changed;

            Rois = new List<Roi>
            {
                roi1, roi2, roi3
            };
        }

        private void OnRoi1Changed(object sender, Roi e)
        {
            Console.WriteLine($@"Roi1, x: {e.X}, y: {e.Y}");
            if (!e.Show)
            {
                Console.WriteLine(@"Roi1 hide/deleted");
            }
        }

        private void OnRoi2Changed(object sender, Roi e)
        {
            Console.WriteLine($@"Roi2, x: {e.X}, y: {e.Y}");
            if (!e.Show)
            {
                Console.WriteLine(@"Roi2 hide/deleted");
            }
        }

        private void OnRoi3Changed(object sender, Roi e)
        {
            Console.WriteLine($@"Roi3, x: {e.X}, y: {e.Y}");
            if (!e.Show)
            {
                Console.WriteLine(@"Roi3 hide/deleted");
            }
        }
    }
}
