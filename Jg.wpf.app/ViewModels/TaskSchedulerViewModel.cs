using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;
using Jg.wpf.core.Service;
using Jg.wpf.core.Service.ThreadService;

namespace Jg.wpf.app.ViewModels
{
    public class TaskSchedulerViewModel : ViewModelBase
    {
        private readonly TaskItemsManager _taskItemsManager;

        public TaskItemsManager TaskItemsManager => _taskItemsManager;

        public JCommand StartNewTaskCommand { get; }



        public TaskSchedulerViewModel()
        {
            _taskItemsManager = new TaskItemsManager();

            StartNewTaskCommand = new JCommand("StartNewTaskCommand", OnStartNewTask);

        }

        private void OnStartNewTask(object obj)
        {
            lock (LockHelper.Locker)
            {
                var exist = _taskItemsManager.TaskItems.Count;
                var newId = 1;
                if (exist > 0)
                {
                    newId = _taskItemsManager.TaskItems[exist - 1].Id + 1;
                }
                var newTaskItem = new TaskItemViewModel(newId);

                _taskItemsManager.StartNewTask(newTaskItem);
            }
        }


    }

    public class TaskItemViewModel : ViewModelBase
    {
        private int _percent;
        private bool _isPaused;
        private bool _isStart;

        public int Id { get; }
        public int Percent
        {
            get => _percent;
            set
            {
                if (_percent != value)
                {
                    _percent = value;
                    RaisePropertyChanged(nameof(Percent));
                }
            }
        }
        public bool IsStart
        {
            get => _isStart;
            set
            {
                _isStart = value;
                RaisePropertyChanged(nameof(IsStart));
            }
        }

        public TaskProxy Proxy { get; set; }
        public JCommand PauseTaskCommand { get; }
        public JCommand CancelTaskCommand { get; }


        public TaskItemViewModel(int id)
        {
            Id = id;

            PauseTaskCommand = new JCommand("PauseTaskCommand", OnPauseTask);
            CancelTaskCommand = new JCommand("CancelTaskCommand", OnCancelTask);
        }

        private void OnPauseTask(object obj)
        {
            if (_isPaused)
            {
                _isPaused = false;
                Proxy.Restore();
            }
            else
            {
                _isPaused = true;
                Proxy.Suspend();
            }
        }

        private void OnCancelTask(object obj)
        {
            Proxy.Cancel();
        }
    }

    public class TaskItemsManager
    {
        private readonly TaskManager _taskManager;
        private readonly Dictionary<int, TaskProxy> _proxies;

        public ObservableCollection<TaskItemViewModel> TaskItems { get;}

        public TaskItemsManager()
        {
            TaskItems = new ObservableCollection<TaskItemViewModel>();

            _proxies = new Dictionary<int, TaskProxy>();
            _taskManager = new TaskManager("TaskManagerDemo");
        }

        public void StartNewTask(TaskItemViewModel newTask)
        {
            lock (LockHelper.Locker)
            {
                TaskItems.Add(newTask);
                var proxy = _taskManager.StartNewTaskProxy($"TaskDemo{newTask.Id}", OnTaskRunning, null, newTask);
                _proxies[newTask.Id] = proxy;
                newTask.Proxy = proxy;
            }
        }

        private void OnTaskRunning(TaskProxy proxy)
        {
            if (proxy.Tag is TaskItemViewModel taskItem)
            {
                taskItem.IsStart = true;
                while (!proxy.CancelTokenSource.IsCancellationRequested && taskItem.Percent < 100)
                {
                    try
                    {
                        ServiceManager.MainDispatcher.Invoke(() =>
                        {
                            taskItem.Percent += 1;
                        });

                        Thread.Sleep(50);

                        proxy.WaitOne();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                taskItem.IsStart = false;

                Thread.Sleep(200);

                ServiceManager.MainDispatcher.Invoke(() =>
                {
                    TaskItems.Remove(taskItem);
                });

                _proxies.Remove(taskItem.Id);
            }
        }
    }

    public static class LockHelper
    {
        public static object Locker = new object();
    }
}
