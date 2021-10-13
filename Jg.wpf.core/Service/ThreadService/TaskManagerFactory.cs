using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Jg.wpf.core.Service.ThreadService
{
    public class TaskManagerFactory : ITaskManagerFactory
    {
        #region Fields and Events 

        // Fields 

        private readonly Dictionary<string, TaskManager> _taskManagers;

        #endregion Fields and Events 

        #region Properties 

        public static TaskManagerFactory Instance { get; } = new TaskManagerFactory();

        #endregion Properties 

        #region Methods 

        // Constructors 

        private TaskManagerFactory()
        {
            _taskManagers = new Dictionary<string, TaskManager>();
        }

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static TaskManagerFactory()
        {
        }
        // Methods 


        /// <summary>
        /// Gets the task manager from the list, if it does not exist, then a new task manager will be created.
        /// </summary>
        /// <param name="managerName"></param>
        /// <param name="priority"></param>
        /// <param name="affinityMask"></param>
        /// <returns></returns>
        public ITaskManager GetTaskManager(string managerName, ThreadPriority priority = ThreadPriority.Normal, uint affinityMask = 0)
        {
            TaskManager taskManager = null;
            lock (((ICollection)_taskManagers).SyncRoot)
            {
                if (!_taskManagers.TryGetValue(managerName, out taskManager))
                {
                    taskManager = new TaskManager(managerName, priority, affinityMask);
                }
            }
            return taskManager;
        }

        public void CloseTaskManager(string name, bool cancelAllTask)
        {
            TaskManager manager;
            lock (((ICollection)_taskManagers).SyncRoot)
            {
                _taskManagers.TryGetValue(name, out manager);
            }

            if (manager != null)
            {
                if (cancelAllTask)
                {
                    manager.CancelAllTasks();
                }
                manager.Close();
            }
        }

        public void Close()
        {
            List<TaskManager> taskManagers;
            lock (((ICollection)_taskManagers).SyncRoot)
            {
                taskManagers = _taskManagers.Values.ToList();
            }

            foreach (var taskManager in taskManagers)
            {
                var name = taskManager.Name;
                taskManager.Close();
            }
        }

        internal void RegisterTaskManager(TaskManager taskManager)
        {
            lock (((ICollection)_taskManagers).SyncRoot)
            {
                if (_taskManagers.ContainsKey(taskManager.Name))
                {
                }
                else
                {
                    _taskManagers.Add(taskManager.Name, taskManager);
                }
            }
        }

        internal void UnregisterTaskManager(TaskManager taskManager)
        {
            lock (((ICollection)_taskManagers).SyncRoot)
            {
                _taskManagers.Remove(taskManager.Name);
            }
        }

        #endregion Methods 
    }
}
