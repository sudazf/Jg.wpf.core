using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jg.wpf.core.Service.ThreadService
{
    public interface ITaskManager
    {
        string Name { get; }

        /// <summary>
        /// Gets the number of tasks in the queue.
        /// </summary>
        int TaskCount { get; }

        Task[] Tasks { get; }

        /// <summary>
        /// Request to cancel all tasks.
        /// </summary>
        /// <remarks>Some tasks will not be cancelled immediately, so please don't rely on the status immediately after calling this method</remarks>
        void CancelAllTasks();

        /// <summary>
        /// Create a TaskProxy
        /// </summary>
        /// <param name="taskName">The human readable name of the task</param>
        /// <param name="action">The Action to be executed when task is running</param>
        /// <param name="continueWithAction">The optional action to be executed after the task is completed, either finished, cancelled or error happens.</param>
        /// <returns>A new instance of <c>TaskProxy</c></returns>
        TaskProxy StartNewTaskProxy(string taskName, Action action, Action<TaskProxy> continueWithAction = null);

        /// <summary>
        /// Create a TaskProxy
        /// </summary>
        /// <param name="taskName">The human readable name of the task</param>
        /// <param name="action">The Action to be executed when task is running</param>
        /// <param name="continueWithAction">The optional action to be executed after the task is completed, either finished, cancelled or error happens.</param>
        /// <param name="tag">An optional data which can be passed to the Action. </param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns>A new instance of <c>TaskProxy</c></returns>
        /// <remarks>The tag passed to the method is stored as <c>Tag</c> property of TaskProxy.</remarks>
        TaskProxy StartNewTaskProxy(string taskName, Action<TaskProxy> action,
            Action<TaskProxy> continueWithAction = null, object tag = null,
            CancellationTokenSource cancellationTokenSource = null);

        TaskProxy StartNewTaskProxy(string taskName, Action<object> action, object tag,
            Action<TaskProxy> continueWithAction = null);
        /// <summary>
        /// Wait until all tasks are completed, then close the task manager.
        /// </summary>
        /// <remarks>After task manager is closed, it shall not be use any more.</remarks>
        void Close();

        event EventHandler Idle;
    }

    public interface ITaskManagerFactory
    {
        ITaskManager GetTaskManager(string managerName, ThreadPriority priority = ThreadPriority.Normal, uint affinityMask = 0);
    }
}
