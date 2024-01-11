using Jg.wpf.core.Log;
using System;

namespace Jg.wpf.core.Service.Performance
{
    public class TimePerformance : ProfilerBase, IDisposable
    {
        private readonly CpuClock _enterTime = new CpuClock();
        private readonly double _threshold;

        public double TotalMilliSeconds => _enterTime.TotalMilliSeconds;
        /// <summary>
        /// Create a time performance.
        /// </summary>
        /// <param name="name">Current task name.</param>
        /// <param name="threshold">the warning limit of time (in millisecond).</param>
        public TimePerformance(string name, double threshold) : base(name)
        {
            _threshold = threshold;
        }

        public void Dispose()
        {
            var elapsedMilliSeconds = _enterTime.TotalMilliSeconds;
            if (elapsedMilliSeconds >= _threshold)
            {
                var message = $"Time performance warning: {Message} expect less than {_threshold} ms, exactly cost {elapsedMilliSeconds} ms.";
                Console.WriteLine(message);
                //Logger.WriteLineInfo(message);
            }
        }
    }
}
