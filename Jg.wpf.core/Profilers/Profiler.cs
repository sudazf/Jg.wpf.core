using System;

namespace Jg.wpf.core.Profilers
{
    public class Profiler
    {
        public static ProfilerEnum ProfilerLevel { get; set; }
    }


    [Flags]
    public enum ProfilerEnum
    {
        AuditTrial = 0,
        Performance = 0x1,
        Memory = 0x2,
    }
}
