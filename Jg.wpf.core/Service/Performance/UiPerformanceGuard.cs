using System.Windows.Threading;
using System.Windows;

namespace Jg.wpf.core.Service.Performance
{
    public class UiPerformanceGuard
    {
        private readonly int _performanceThreshold;

        private Dispatcher _threadDispatcher;
        private FrameworkElement _loadingIndicator;
        private TimePerformance _timePerformance;

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
        }

        public void Start(string workingName)
        {
            _timePerformance = new TimePerformance(workingName, _performanceThreshold);

            _threadDispatcher.Invoke(() =>
            {
                _loadingIndicator.Visibility = Visibility.Visible;
            });
        }

        public void Stop()
        {
            _timePerformance.Dispose();

            if ( _loadingIndicator != null && _threadDispatcher != null)
            {
                _threadDispatcher.Invoke(() =>
                {
                    _loadingIndicator.Visibility = Visibility.Collapsed;
                });
            }
        }
    }
}
