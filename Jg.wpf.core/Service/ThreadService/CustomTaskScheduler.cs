using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace Jg.wpf.core.Service.ThreadService
{
    internal class CustomTaskScheduler : TaskScheduler, IDisposable
    {
        private readonly TaskManager _taskManager;
        private readonly string _name;
        private readonly uint _affinityMask;
        private readonly List<Task> _currentTasks = new List<Task>();
        private readonly ManualResetEvent[] _schedulerEvents = new ManualResetEvent[2];
        private readonly ManualResetEvent _threadCompleteEvent = new ManualResetEvent(false);
        private static readonly object Mutex = new object();


        public CustomTaskScheduler(TaskManager taskManager, string name, ThreadPriority priority = ThreadPriority.Normal, uint affinityMask = 0)
        {
            _taskManager = taskManager;
            _name = name;
            _affinityMask = affinityMask;
            _schedulerEvents[0] = new ManualResetEvent(false);
            _schedulerEvents[1] = new ManualResetEvent(false);
            Thread = ThreadManager.Create($"SolarUIScheduleThread<{name}>", ExecutionThread, null, priority);
        }

        public Thread Thread { get; }

        private uint ExecutionThread(object args)
        {
            IThreadAffinityMaskSetter setter = null;
            var affinityMask = _affinityMask;
            if (_affinityMask != 0)
            {
                setter = new ThreadAffinityMaskSetter(affinityMask);
            }
            try
            {
                while (true)
                {
                    if (!WaitIfEmpty())
                    {
                        break;
                    }

                    Task task;
                    lock (Mutex)
                    {
                        task = _currentTasks[0];
                    }

                    try
                    {
                        TryExecuteTask(task);
                    }
                    finally
                    {
                        TryDequeue(task);
                    }
                }
            }
            finally
            {
                // Before thread is over, signal so that callers can continue.
                _threadCompleteEvent.Set();
                setter?.Close();
            }
            return 0;
        }

        private bool WaitIfEmpty()
        {
            bool inIdle = false;
            lock (Mutex)
            {
                if (_currentTasks.Count == 0)
                {
                    // Start task
                    _schedulerEvents[0].Reset();
                    inIdle = true;
                }
            }
            if (inIdle)
            {
                _taskManager.OnIdle();
            }

            int id = WaitHandle.WaitAny(_schedulerEvents);

            // when id is 0, new thread is added, when id is not 0, thread is terminated
            return id == 0;
        }

        protected override void QueueTask(Task task)
        {
            lock (Mutex)
            {
                _currentTasks.Add(task);
                if (_currentTasks.Count > 0)
                {
                    // Start task
                    _schedulerEvents[0].Set();
                }
            }
        }

        [SecurityCritical]
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        /// <summary>Attempts to remove a previously scheduled task from the scheduler.</summary>
        /// <param name="task">The task to be removed.</param>
        /// <returns>Whether the task could be found and removed.</returns>
        protected sealed override bool TryDequeue(Task task)
        {
            lock (Mutex)
            {
                return _currentTasks.Remove(task);
            }
        }

        public override int MaximumConcurrencyLevel => 1;

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (Mutex)
            {
                return _currentTasks.ToArray();
            }
        }

        public int Tasks
        {
            get
            {
                lock (Mutex)
                {
                    return _currentTasks.Count;
                }
            }
        }

        public void Dispose()
        {
            _schedulerEvents[1].Set();
        }
    }

    internal interface IThreadAffinityMaskSetter
    {
        void Close();
    }
    internal class ThreadAffinityMaskSetter : IThreadAffinityMaskSetter
    {
        private readonly uint _mask;

        internal ThreadAffinityMaskSetter(uint mask)
        {
            _mask = mask;
            Begin();
        }

        private void Begin()
        {
            if (_mask != 0)
            {
                IntPtr osThread = Native32.GetCurrentThread();

                //Mask is whatever processor(s) you want to run it on in this case 
                UIntPtr mask = new UIntPtr(_mask);
                UIntPtr affinity = Native32.SetThreadAffinityMask(osThread, mask);
                if (affinity == UIntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
                Thread.BeginThreadAffinity();
            }
        }

        public void Close()
        {
            if (_mask != 0)
            {
                Thread.EndThreadAffinity();
            }
        }

        static class Native32
        {
            [DllImport("kernel32.dll")]
            public static extern IntPtr GetCurrentThread();

            [DllImport("kernel32")]
            public static extern int GetCurrentThreadId();

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetCurrentProcessorNumber();

            [HostProtection(SelfAffectingThreading = true)]
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern UIntPtr SetThreadAffinityMask(IntPtr handle, UIntPtr mask);
        }
    }
}
