using System;
using System.Collections.ObjectModel;

namespace Jg.wpf.core.Extensions.Collections
{

    public class MyObservableCollection<T> : ObservableCollection<T>
    {
        public Action<MyObservableCollection<T>> ClearInvokeAction { get; set; }

        /// <summary>
        /// 清除前，释放掉一些会造成内存泄露的资源，如订阅的事件等。
        /// </summary>
        public void ClearEx()
        {
            ClearInvokeAction(this);

            base.Clear();
        }
    }
}
