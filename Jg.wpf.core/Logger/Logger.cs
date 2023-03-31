using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Jg.wpf.core.Profilers;
using Jg.wpf.core.Service.Resource;

namespace Jg.wpf.core.Log
{
    public class Logger
    {
        private static TraceLevel _traceLevel;
        private static readonly object Mutex = new object();
        private const double BytesPerGB = 1000 * 1000 * 1024;
        public static bool LogTime { get; set; }

        public static Dictionary<string, TraceLevel> AssemblyFilters { get; } = new Dictionary<string, TraceLevel>();

        public static bool Filter { get; set; }

        public static string FileName { get; set; }

        public static bool Ignore { get; set; }

        public static bool IgnoreNative { get; set; }

        public static TraceLevel LogLevel
        {
            get => _traceLevel;
            set
            {
                if (_traceLevel != value)
                {
                    lock (Mutex)
                    {
                        _traceLevel = value;
                    }
                }
            }
        }

        [ThreadStatic]
        public static TraceLevel CurrentLoggertraceLevel;

        public static bool ShowPath { get; set; }
        public static LogType ShowLogType { get; set; }

        static Logger()
        {
            LogTime = true;
        }
        private static readonly string _infoTag = "I ; ";
        private static readonly string _warnTag = "W ; ";
        private static readonly string _errorTag = "E ; ";
        private static readonly string _verboseTag = "V ; ";
        private const string NativeLogHeader = "NAT:";

        private static int _lastFlush;
        private static int _logFlushLevel = -1;

        public static void Initialize()
        {
            Logger.LogLevel = TraceLevel.Info;
            DateTime currentTime = DateTime.Now;
            string newFileName = $"{currentTime:yyyyMMdd_HHmmss}.log";

            var logDirectory = ResourceManager.GetValue("LogSettings", "Directory", @"d:\SolarLogs");

            if (!Directory.Exists(logDirectory))
            {
                var desireDisk = Path.GetPathRoot(logDirectory);
                if (string.IsNullOrEmpty(desireDisk))
                {
                    logDirectory = @"C:\SolarLogs";
                }
                else
                {
                    var disks = (IList)Directory.GetLogicalDrives();
                    if (!disks.Contains(desireDisk.ToUpper()))
                    {
                        logDirectory = @"C:\SolarLogs";
                    }
                }

                Directory.CreateDirectory(logDirectory);
            }

            var fullName = Path.Combine(logDirectory, newFileName);
            var listener = new TextWriterTraceListener(fullName, "myListener");
            Trace.Listeners.Add(listener);
            Logger.FileName = fullName;
        }

        public static void WriteLineError(string message)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Error, message, null, 2);
        }
        public static void WriteLineError(string format, object arg1)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Error, format, new[] { arg1 }, 2);
        }
        public static void WriteLineError(string format, object arg1, object arg2)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Error, format, new[] { arg1, arg2 }, 2);
        }
        public static void WriteLineError(string format, params object[] args)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Error, format, args, 2);
        }

        [Conditional("DEBUG")]
        public static void WriteLineWarning(string message)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Warning, message, null, 2);
        }
        [Conditional("DEBUG")]
        public static void WriteLineWarning(string format, object arg1)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Warning, format, new[] { arg1 }, 2);
        }
        [Conditional("DEBUG")]
        public static void WriteLineWarning(string format, object arg1, object arg2)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Warning, format, new[] { arg1, arg2 }, 2);
        }
        [Conditional("DEBUG")]
        public static void WriteLineWarning(string format, params object[] args)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Warning, format, args, 2);
        }

        [Conditional("DEBUG")]
        public static void WriteLineInfo(string message)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Info, message, null, 2);
        }
        [Conditional("DEBUG")]
        public static void WriteLineInfo(string format, object arg1)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Info, format, new[] { arg1 }, 2);
        }
        [Conditional("DEBUG")]
        public static void WriteLineInfo(string format, object arg1, object arg2)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Info, format, new[] { arg1, arg2 }, 2);
        }

        [Conditional("DEBUG")]
        public static void WriteLineInfo(string format, params object[] args)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Info, format, args, 2);
        }

        [Conditional("DEBUG")]
        public static void WriteLineVerbose(string message)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Verbose, message, null, 2);
        }
        [Conditional("DEBUG")]
        public static void WriteLineVerbose(string format, object arg1)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Verbose, format, new[] { arg1 }, 2);
        }
        [Conditional("DEBUG")]
        public static void WriteLineVerbose(string format, object arg1, object arg2)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Verbose, format, new[] { arg1, arg2 }, 2);
        }

        [Conditional("DEBUG")]
        public static void WriteLineVerbose(string format, params object[] args)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Verbose, format, args, 2);
        }

        public static void ForceWriteLine(string message)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Off, message, null, 2);
        }

        public static void ForceWriteLine(string format, object arg1)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Off, format, new[] { arg1 }, 2);
        }

        public static void ForceWriteLine(string format, object arg1, object arg2)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Off, format, new[] { arg1, arg2 }, 2);
        }

        public static void ForceWriteLine(string format, params object[] args)
        {
            WriteLineIf(ShowLogType != LogType.Native, TraceLevel.Off, format, args, 2);
        }

        public static void ForceWriteLineIf(bool condition, string format, params object[] args)
        {
            WriteLineIf(condition && ShowLogType != LogType.Native, TraceLevel.Off, format, args, 2);
        }

        [Conditional("DEBUG")]
        public static void WriteLineInfoIf(bool condition, string format, params object[] args)
        {
            WriteLineIf(condition, TraceLevel.Info, format, args, 2);
        }

        [Conditional("DEBUG")]
        public static void WriteLineWarningIf(bool condition, string format, params object[] args)
        {
            WriteLineIf(condition, TraceLevel.Warning, format, args, 2);
        }

        public static void WriteLineErrorIf(bool condition, string format, params object[] args)
        {
            WriteLineIf(condition, TraceLevel.Error, format, args, 2);
        }

        [Conditional("DEBUG")]
        public static void WriteLineVerboseIf(bool condition, string format, params object[] args)
        {
            WriteLineIf(condition, TraceLevel.Verbose, format, args, 2);
        }

        private static void WriteLineIf(bool condition, TraceLevel traceLevel, string format, object[] args, int stackLevel)
        {
            if ((!condition) || (ShowLogType == LogType.Native)) return;

            WriteLineIf(traceLevel, format, args, stackLevel);
        }

        public static void WriteNativeLogIf(TraceLevel traceLevel, string format, object[] args)
        {
            if (ShowLogType == LogType.DotNet)
            {
                return;
            }
            WriteLineIf(traceLevel, format, args, 2);
        }

        private static readonly MemoryMonitor GlobalMonitor = new MemoryMonitor();

        private static void WriteLineIf(TraceLevel traceLevel, string format, object[] args, int stackLevel = 1)
        {
            if (traceLevel <= _traceLevel)
            {
                try
                {
                    var now = DateTime.Now;

                    StringBuilder sb = new StringBuilder();

                    if (LogTime)
                    {
                        int threadId = Thread.CurrentThread.ManagedThreadId;

                        sb.AppendFormat("[{0:HH:mm:ss},{1:d3}]({2})-", now, now.Millisecond, threadId);
                    }
                    string message = args == null ? format : string.Format(format, args);

                    if (traceLevel == TraceLevel.Error)
                    {
                        sb.Append(_errorTag);
                    }
                    else if (traceLevel == TraceLevel.Warning)
                    {
                        sb.Append(_warnTag);
                    }
                    else if (traceLevel == TraceLevel.Info)
                    {
                        sb.Append(_infoTag);
                    }
                    else if (traceLevel == TraceLevel.Verbose)
                    {
                        sb.Append(_verboseTag);
                    }

                    sb.Append(message);
                    if ((Profiler.ProfilerLevel & ProfilerEnum.Memory) != 0)
                    {
                        sb.AppendFormat(". Mem:{0:F2} MB", GlobalMonitor.Delta);
                    }

                    CurrentLoggertraceLevel = traceLevel; //Set trace level, Tracelistener need to get the infor
                    // TODO, later we can implement other logger modes(e.g., log to file, log according to log listeners, ...)                       
                    Trace.WriteLine(sb.ToString());
                }
                catch (Exception)
                {
                    // There is exception during trace, so just ignore
                }

                if (traceLevel <= TraceLevel.Info)
                {
                    int lastFlush = Environment.TickCount;

                    var previousFlush = _lastFlush;
                    if (traceLevel <= TraceLevel.Error ||
                        previousFlush == 0 ||
                        (lastFlush - previousFlush >= 1000)
                        )
                    {
                        if (_logFlushLevel == -1)
                        {
                            _logFlushLevel = 1;
                        }
                        if (_logFlushLevel == 1)
                        {
                            // Avoid flush too frequently
                            Flush();
                        }

                        lock (Mutex)
                        {
                            _lastFlush = lastFlush;
                        }
                    }
                }
            }
        }

        public static void Flush()
        {
            lock (Mutex)
            {
                try
                {
                    // Auto Flush the error level
                    foreach (TraceListener listener in Trace.Listeners)
                    {
                        listener.Flush();
                    }
                    _lastFlush = Environment.TickCount;
                }
                catch (IOException)
                {

                }
            }
        }
    }

    public enum LogType
    {
        Both,
        DotNet,
        Native
    }
}
