using System.Threading;
using System.Windows.Threading;
using System.Windows;
using System;

namespace Jg.wpf.core.Service.Performance
{
    public class UiPerformanceGuard
    {
        private readonly int _performanceThreshold;

        private Dispatcher _threadDispatcher;
        private FrameworkElement _loadingIndicator;
        private DispatcherTimer _dispatcherTimer;
        private TimePerformance _timePerformance;
        private DateTime _startTime;

        public UiPerformanceGuard(int threshold = 100)
        {
            _performanceThreshold = threshold;
        }

        /// <summary>
        /// Init after app loaded.
        /// </summary>
        public void Init(Dispatcher threadDispatcher, FrameworkElement indicator)
        {
            _threadDispatcher = threadDispatcher;
            _loadingIndicator = indicator;

            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background, _threadDispatcher);
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(_performanceThreshold);
            _dispatcherTimer.Tick += DispatcherTimerTick;
        }

        public void Start(string workingName)
        {
            _timePerformance = new TimePerformance(workingName, _performanceThreshold);
            _dispatcherTimer?.Start();
        }

        public void Stop()
        {
            _timePerformance.Dispose();

            if (_dispatcherTimer != null && _loadingIndicator != null && _threadDispatcher != null)
            {
                _dispatcherTimer.Stop();

                _threadDispatcher.Invoke(() =>
                {
                    _loadingIndicator.Visibility = Visibility.Collapsed;
                });
            }
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            _startTime = DateTime.Now;
            _loadingIndicator.Visibility = Visibility.Visible;
            _dispatcherTimer.Stop();
        }
    }

}
