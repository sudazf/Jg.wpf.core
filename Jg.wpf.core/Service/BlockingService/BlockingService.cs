using Jg.wpf.core.Service.ThreadService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jg.wpf.core.Service.BlockingService
{
    public class BlockingService<T>
    {
        private readonly BlockingCollection<T> _collection;

        /// <summary>
        /// 表示每自动分发一个集合元素，将处理该事件的方法
        /// </summary>
        public event EventHandler<T> OnTakingOutItem;

        public BlockingCollection<T> BlockingCollection => _collection;

        /// <summary>
        /// 创建带 自动异步分发功能 的集合服务
        /// </summary>
        public BlockingService()
        {
            _collection = new BlockingCollection<T>();
        }

        /// <summary>
        /// 将需要分发的单个元素添加到集合服务中
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            _collection.Add(item);
        }

        public void Start()
        {
            ThreadManager.Create($"BlockingService-{Guid.NewGuid().ToString()}", OnExecuteThread, null);
        }

        public void Stop()
        {
            _collection.CompleteAdding();
        }


        private uint OnExecuteThread(object arg)
        {
            foreach (var item in _collection.GetConsumingEnumerable())
            {
                OnTakingOutItem?.Invoke(this, item);
            }

            return 0;
        }

    }
}
