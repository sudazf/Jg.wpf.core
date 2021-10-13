using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jg.wpf.core.Service.ThreadService
{
    public class TaskManager : ITaskManager
    {
        public string Name { get; }
        private readonly Dictionary<Task, TaskProxy> _currentTasks = new Dictionary<Task, TaskProxy>();
        private readonly TaskFactory _taskFactory;
        private readonly CustomTaskScheduler _scheduler;

        public event EventHandler Idle;
        public TaskManager(string name, ThreadPriority priority = ThreadPriority.Normal, uint affinityMask = 0)
        {
            Name = name;
            _scheduler = new CustomTaskScheduler(this, name, priority, affinityMask);
            _taskFactory = new TaskFactory(_scheduler);

            TaskManagerFactory.Instance.RegisterTaskManager(this);
        }

        internal void OnIdle()
        {
            var handler = Idle;
            handler?.Invoke(this, EventArgs.Empty);
        }

        internal TaskScheduler Scheduler => _scheduler;

        public int TaskCount
        {
            get
            {
                lock (_scheduler)
                {
                    return _currentTasks.Count;
                }
            }
        }

        public Task[] Tasks
        {
            get
            {
                lock (_scheduler)
                {
                    return _currentTasks.Keys.ToArray();
                }
            }
        }

        public void CancelAllTasks()
        {
            TaskProxy[] proxies;
            lock (_scheduler)
            {
                proxies = _currentTasks.Values.ToArray();
            }

            foreach (var proxy in proxies)
            {
                proxy.Cancel();
            }

            lock (_scheduler)
            {
                _currentTasks.Clear();
            }
        }

        /// <summary>
        /// 创建并运行一个TaskProxy
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="action">执行Action</param>
        /// <param name="tag"></param>
        /// <param name="continueWithAction"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public TaskProxy StartNewTaskProxy(string taskName, Action<TaskProxy> action, Action<TaskProxy> continueWithAction = null, object tag = null, CancellationTokenSource cancellationTokenSource = null)
        {
            if (cancellationTokenSource == null)
            {
                cancellationTokenSource = new CancellationTokenSource();
            }

            lock (_scheduler)
            {
                var proxy = new TaskProxy(taskName, cancellationTokenSource)
                { Tag = tag, ContinueWith = continueWithAction };
                Task task = _taskFactory.StartNew(
                    TaskInvokeHelper,
                    new TaskProxyContext(proxy, action),
                    cancellationTokenSource.Token,
                    TaskCreationOptions.None,
                    _scheduler);
                proxy.Task = task;
                _currentTasks[task] = proxy;
                task.ContinueWith(InnerContinueWith, _scheduler);
                return proxy;
            }
        }

        private void TaskInvokeHelper(object state)
        {
            TaskProxyContext taskProxyContext = (TaskProxyContext)state;
            taskProxyContext.Action(taskProxyContext.Proxy);
        }

        /// <summary>
        /// 创建并运行一个TaskProxy
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="action">执行Action</param>
        /// <param name="continueWithAction">Task执行完毕后执行Action</param>
        /// <returns></returns>
        public TaskProxy StartNewTaskProxy(string taskName, Action action, Action<TaskProxy> continueWithAction = null)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lock (_scheduler)
            {
                var proxy = new TaskProxy(taskName, cancellationTokenSource);
                Task task = _taskFactory.StartNew(
                    action,
                  cancellationTokenSource.Token,
                  TaskCreationOptions.None,
                  _scheduler);
                proxy.ContinueWith = continueWithAction;
                proxy.Task = task;
                _currentTasks[task] = proxy;
                task.ContinueWith(InnerContinueWith, _scheduler);
                return proxy;
            }
        }

        /// <summary>
        /// 创建并运行一个TaskProxy
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="action">执行Action</param>
        /// <param name="continueWithAction">Task执行完毕后执行Action</param>
        /// <returns></returns>
        public TaskProxy StartNewTaskProxy(string taskName, Action<object> action, object tag, Action<TaskProxy> continueWithAction = null)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            lock (_scheduler)
            {
                var proxy = new TaskProxy(taskName, cancellationTokenSource);
                Task task = _taskFactory.StartNew(
                    action,
                    tag,
                  cancellationTokenSource.Token,
                  TaskCreationOptions.None,
                  _scheduler);
                proxy.ContinueWith = continueWithAction;
                proxy.Task = task;
                _currentTasks[task] = proxy;
                task.ContinueWith(InnerContinueWith, _scheduler);
                return proxy;
            }
        }

        private void InnerContinueWith(Task task)
        {
            TaskProxy taskProxy = null;
            lock (_scheduler)
            {
                if (_currentTasks.ContainsKey(task))
                {
                    taskProxy = _currentTasks[task];
                    _currentTasks.Remove(task);
                }
            }

            taskProxy?.ContinueWith?.Invoke(taskProxy);
        }

        /// <summary>
        /// Wait until all tasks are completed, then close the task manager.
        /// </summary>
        /// <remarks>After task manager is closed, it shall not be use any more.</remarks>
        public virtual void Close()
        {
            CancelAllTasks();

            if (_scheduler is IDisposable disposable)
            {
                disposable.Dispose();
            }

            TaskManagerFactory.Instance.UnregisterTaskManager(this);
        }
    }

    internal class TaskProxyContext
    {
        public TaskProxyContext(TaskProxy proxy, Action<TaskProxy> action)
        {
            Proxy = proxy;
            Action = action;
        }
        public TaskProxy Proxy { get; }
        public Action<TaskProxy> Action { get; }
    }
}
