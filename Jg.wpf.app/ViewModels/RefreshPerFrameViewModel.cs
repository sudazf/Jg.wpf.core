using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class RefreshPerFrameViewModel : ViewModelBase
    {
        private int _refreshRate;
        private bool _canStart;
        private List<BitmapImage> _image2;

        private Thread _pushImageThread;
        private Thread _pushImageThread2;

        public ObservableCollection<BitmapImage> Images { get; set; }
        public ObservableCollection<BitmapImage> Images2 { get; set; }

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
        public JCommand StartCommand2 { get; }
        public JCommand StopCommand { get; }

        public RefreshPerFrameViewModel()
        {
            Images = new ObservableCollection<BitmapImage>();
            _image2 = new List<BitmapImage>();
            Images2 = new ObservableCollection<BitmapImage>();

            RefreshRate = 1000;
            _canStart = true;

            StartCommand = new JCommand("StartRefreshCommand", OnStart, CanStart);
            StopCommand = new JCommand("StopRefreshCommand", OnStop, CanStop);

            StartCommand2 = new JCommand("StartRefreshCommand2", OnStart2, CanStart);
        }

        public void Refresh()
        {
            var image2Count = Images2.Count;
            var innerImage2Count = _image2.Count;

            if (image2Count != innerImage2Count)
            {
                if (image2Count > innerImage2Count)
                {
                    Images2.Clear();

                    var cache = new List<BitmapImage>(_image2);
                    foreach (var images in cache)
                    {
                        Images2.Add(images);
                    }
                }
                else
                {
                    var cache = new List<BitmapImage>(_image2);
                    var ex = cache.Except(Images2);
                    foreach (var image in ex)
                    {
                        Images2.Add(image);
                    }
                }
            }
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

        private void OnStart2(object obj)
        {
            _pushImageThread2 = new Thread(OnPushImage2);
            _pushImageThread2.Start();

            _canStart = false;
            UpdateButtonState();
        }

        private void OnPushImage2()
        {
            while (!_canStart)
            {
                GenerateImageSource2();

                Thread.Sleep(_refreshRate);
            }
        }

        private void OnPushImage()
        {
            while (!_canStart)
            {
                GenerateImageSource();

                Thread.Sleep(_refreshRate);
            }
        }

        private void OnStop(object obj)
        {
            _canStart = true;
            UpdateButtonState();
        }

        private void GenerateImageSource()
        {
            BitmapImage stationImage = new BitmapImage();
            stationImage.BeginInit();
            stationImage.CacheOption = BitmapCacheOption.OnLoad;
            stationImage.UriSource = new Uri("pack://application:,,,/Jg.wpf.app;Component/Resources/Image/duke.jpg", UriKind.RelativeOrAbsolute);
            stationImage.EndInit();
            stationImage.Freeze();

            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (Images.Count > 50)
                {
                    Images.Clear();
                }
                Images.Add(stationImage);
            }));
        }

        private void GenerateImageSource2()
        {
            BitmapImage stationImage = new BitmapImage();
            stationImage.BeginInit();
            stationImage.CacheOption = BitmapCacheOption.OnLoad;
            stationImage.UriSource = new Uri("pack://application:,,,/Jg.wpf.app;Component/Resources/Image/duke.jpg", UriKind.RelativeOrAbsolute);
            stationImage.EndInit();
            stationImage.Freeze();

            if (_image2.Count > 50)
            {
                _image2.Clear();
            }
            _image2.Add(stationImage);
        }

        private void UpdateButtonState()
        {
            StartCommand.RaiseCanExecuteChanged();
            StartCommand2.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
        }

    }
}
