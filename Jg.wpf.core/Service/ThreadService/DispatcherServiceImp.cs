using System;
using System.Windows;
using System.Windows.Threading;
using Jg.wpf.core.Profilers;

namespace Jg.wpf.core.Service.ThreadService
{
    internal class DispatcherServiceImp : IDispatcher
    {
        public object Dispatcher { get; }
        public void Invoke(Action action)
        {
            if (Application.Current == null)
            {
                return;
            }
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(action);
            }
        }
        public void BeginInvoke(Action action)
        {
            if (Application.Current == null)
            {
                return;
            }
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(action);
            }
        }
        public void BeginInvoke(Action<object> action, object para)
        {
            if (Application.Current == null)
            {
                return;
            }
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action(para);
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(action, para);
            }
        }
        public void Invoke(Action<object, object> action, object para1, object para2)
        {
            if (Application.Current == null)
            {
                return;
            }
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action(para1, para2);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(action, para1, para2);
            }
        }
        public bool CheckAccess()
        {
            return Application.Current.Dispatcher.CheckAccess();
        }
        public void DoEvents()
        {
            DispatcherHelper.DoEvents();
        }
        public IDispatcherTimer CreateTimer(TimeSpan interval,
            VDispatcherPriority priority = VDispatcherPriority.Background)
        {
            return new TimerWrapper((Dispatcher)Dispatcher, priority) { Interval = interval };
        }
        public DispatcherServiceImp(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }
    }

    internal class TimerWrapper : IContainer<DispatcherTimer>, IDispatcherTimer
    {
        public TimerWrapper(Dispatcher impl, VDispatcherPriority priority)
        {
            Control = new DispatcherTimer((DispatcherPriority)priority, impl);
        }

        public DispatcherTimer Control { get; }
        public void Start()
        {
            Control.Start();
        }

        public event EventHandler Tick
        {
            add => Control.Tick += value;
            remove => Control.Tick -= value;
        }
        public TimeSpan Interval
        {
            get => Control.Interval;
            set => Control.Interval = value;
        }
        public bool IsEnabled
        {
            get => Control.IsEnabled;
            set => Control.IsEnabled = value;
        }
        public object Tag
        {
            get => Control.Tag;
            set => Control.Tag = value;
        }
        public void Stop()
        {
            Control.Stop();
        }
    }

    public interface IContainer<out T>
    {
        T Control { get; }
    }
}
