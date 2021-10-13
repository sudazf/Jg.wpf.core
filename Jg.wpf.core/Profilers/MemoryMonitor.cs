using System.Diagnostics;

namespace Jg.wpf.core.Profilers
{
    public class MemoryMonitor
    {
        private readonly double _originalMemoryInByte;
        private const double Mb = 1000 * 1024;

        public MemoryMonitor()
        {
            _originalMemoryInByte = Process.GetCurrentProcess().PrivateMemorySize64;
        }

        /// <summary>
        /// The delta MB since monitor is created
        /// </summary>
        public double Delta
        {
            get
            {
                var newValue = Process.GetCurrentProcess().PrivateMemorySize64;
                double delta = (newValue - _originalMemoryInByte) / Mb;
                return delta;
            }
        }
    }
}
