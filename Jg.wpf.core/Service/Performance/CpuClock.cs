using System.Runtime.InteropServices;

namespace Jg.wpf.core.Service.Performance
{
    public class CpuClock
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private static readonly double CpuFrequency;
        private readonly double _startTime;

        public static CpuClock Now => new CpuClock();

        public static double Current
        {
            get
            {
                QueryPerformanceCounter(out var newCounter);
                return newCounter / CpuFrequency;
            }
        }

        public double StartTime => _startTime;
        public double TotalSeconds => (Current - _startTime);
        public double TotalMilliSeconds => (Current - _startTime) * 1000;

        public CpuClock()
        {
            QueryPerformanceCounter(out var counter);
            _startTime = counter / CpuFrequency;
        }

        static CpuClock()
        {
            QueryPerformanceFrequency(out var counter);
            CpuFrequency = counter;
        }

        public double GetTime()
        {
            QueryPerformanceCounter(out var newCounter);
            return (newCounter) / CpuFrequency - _startTime;
        }
    }
}
