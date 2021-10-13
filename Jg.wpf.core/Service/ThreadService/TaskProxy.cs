using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jg.wpf.core.Service.ThreadService
{
    public sealed class TaskProxy
    {
        private double _progress;
        private readonly ManualResetEvent _manualResetEvent;

        public Task Task { get; set; }

        public CancellationTokenSource CancelTokenSource { get; private set; }

        public static TaskProxy CompletedTaskProxy => new TaskProxy("CompletedTaskProxy", new CancellationTokenSource())
        {
            Task = Task.CompletedTask
        };

        public TaskProxy(string name, CancellationTokenSource taskTokenSource)
        {
            Name = name;
            CancelTokenSource = taskTokenSource ?? throw new ArgumentNullException("TaskTokenSource");
            _manualResetEvent = new ManualResetEvent(true);
        }

        public void Wait()
        {
            if (Task == null) throw new InvalidOperationException("Task is not set");
            Task.Wait();
        }

        //true if the Task completed execution within the allotted time; otherwise, false.
        public bool Wait(TimeSpan timeSpan)
        {
            if (Task == null) throw new InvalidOperationException("Task is not set");
            return Task.Wait(timeSpan);
        }

        public void ThrowIfCancellationRequested()
        {
            CancelTokenSource.Token.ThrowIfCancellationRequested();
        }

        public TaskStatus Status
        {
            get
            {
                if (Task == null) throw new InvalidOperationException("Task is not set");
                return Task.Status;
            }
        }

        public bool IsCanceled => CancelTokenSource.IsCancellationRequested;

        public string Name { get; private set; }

        public bool IsCompleted
        {
            get
            {
                if (Task == null) throw new InvalidOperationException("Task is not set");
                return Task.IsCompleted;
            }
        }

        public bool IsFaulted
        {
            get
            {
                if (Task == null) throw new InvalidOperationException("Task is not set");

                return Task.IsFaulted;
            }
        }

        public double Progress
        {
            get => _progress;
            set
            {
                if (Math.Abs(_progress - value) > 0)
                {
                    _progress = value;
                    var handler = ProgressChanged;
                    handler?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Cancel()
        {
            _manualResetEvent.Set();
            CancelTokenSource.Cancel();
        }

        public object Tag { get; set; }

        public Action<TaskProxy> ContinueWith { get; set; }

        public event EventHandler ProgressChanged;

        public void WaitOne()
        {
            _manualResetEvent.WaitOne();
        }
        public void Restore()
        {
            _manualResetEvent.Set();
        }
        public void Suspend()
        {
            _manualResetEvent.Reset();
        }
    }
}
