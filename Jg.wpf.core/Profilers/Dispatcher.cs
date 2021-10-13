using System;

namespace Jg.wpf.core.Profilers
{
    public interface IDispatcher
    {
        object Dispatcher { get; }
        void Invoke(Action action);
        void BeginInvoke(Action action);
        void BeginInvoke(Action<object> action, object para);
        void Invoke(Action<object, object> action, object para1, object para2);
        bool CheckAccess();
        void DoEvents();
        IDispatcherTimer CreateTimer(TimeSpan interval, VDispatcherPriority priority = VDispatcherPriority.Background);
    }
    public interface IDispatcherTimer
    {
        void Start();
        event EventHandler Tick;
        TimeSpan Interval { get; set; }
        bool IsEnabled { get; set; }
        object Tag { get; set; }
        void Stop();
    }

    public enum VDispatcherPriority
    {
        //Invalid = -1,
        //Inactive = 0,
        SystemIdle = 1,
        ApplicationIdle = 2,
        //ContextIdle = 3,
        Background = 4,
        Input = 5,
        Loaded = 6,
        Render = 7,
        //DataBind = 8,
        Normal = 9,
        Send = 10,
    }
}
