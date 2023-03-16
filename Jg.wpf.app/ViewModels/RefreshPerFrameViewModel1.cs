using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Media.Imaging;
using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class RefreshPerFrameViewModel1 : ViewModelBase
    {
        private int _refreshRate;
        private bool _canStart;

        private Thread _pushImageThread;

        public ObservableCollection<BitmapImage> Images { get; set; }

        public int RefreshRate
        {
            get => _refreshRate;
            set
            {
                _refreshRate = value; 
                RaisePropertyChanged(nameof(RefreshRate));
            }
        }

        public JCommand StartCommand { get; }
        public JCommand StopCommand { get; }

        public RefreshPerFrameViewModel1()
        {
            Images = new ObservableCollection<BitmapImage>();

            RefreshRate = 1000;
            _canStart = true;

            StartCommand = new JCommand("StartRefreshCommand", OnStart, CanStart);
            StopCommand = new JCommand("StopRefreshCommand", OnStop, CanStop);
        }

        private bool CanStart(object arg)
        {
            return _canStart;
        }
        private bool CanStop(object arg)
        {
            return !_canStart;
        }
        private void OnStart(object obj)
        {
            _pushImageThread = new Thread(OnPushImage);
            _pushImageThread.Start();

            _canStart = false;
            UpdateButtonState();
        }
        private void OnStop(object obj)
        {
            _canStart = true;
            UpdateButtonState();
        }
        private void OnPushImage()
        {
            while (!_canStart)
            {
                GenerateImageSource();

                Thread.Sleep(_refreshRate);
            }
        }
        private void GenerateImageSource()
        {
            BitmapImage stationImage = new BitmapImage();
            stationImage.BeginInit();
            stationImage.CacheOption = BitmapCacheOption.OnLoad;
            stationImage.UriSource = new Uri("pack://application:,,,/Jg.wpf.app;Component/Resources/Image/duke.jpg", UriKind.RelativeOrAbsolute);
            stationImage.EndInit();
            stationImage.Freeze();

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (Images.Count > 50)
                {
                    Images.Clear();
                }
                Images.Add(stationImage);
            });
        }
        private void UpdateButtonState()
        {
            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

    }
}
