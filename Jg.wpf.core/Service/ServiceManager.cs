using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Jg.wpf.core.Profilers;

namespace Jg.wpf.core.Service
{
    public static class ServiceManager
    {
        private static readonly INamedServiceManager NamedServiceManagerImp = new NamedServiceManagerImp();
        public static IDispatcher MainDispatcher => ServiceManager.GetService<IDispatcher>();

        public static void RegisterService(string name, object service)
        {
            NamedServiceManagerImp.RegisterService(name, service);
        }

        public static object GetService(string name)
        {
            return NamedServiceManagerImp.GetService(name);
        }
        public static T GetService<T>(string name)
        {
            return NamedServiceManagerImp.GetService<T>(name);
        }
        public static T GetService<T>()
        {
            return NamedServiceManagerImp.GetService<T>();
        }

        public static void Init(Dispatcher dispatcher)
        {
            BaseService.Register(dispatcher);
        }
    }

    internal interface INamedServiceManager
    {
        void RegisterService(string name, object service);
        object GetService(string name);
        T GetService<T>(string name);
        T GetService<T>();
    }

    internal class NamedServiceManagerImp : INamedServiceManager
    {
        private readonly Dictionary<string, object> _services = new Dictionary<string, object>();
        private static readonly object Mutex = new object();

        public void RegisterService(string name, object service)
        {
            lock (Mutex)
            {
                _services[name] = service;
            }
        }

        public object GetService(string name)
        {
            return TryGetService(name);
        }
        public T GetService<T>(string name)
        {
            return TryGetService<T>(name);
        }
        public T GetService<T>()
        {
            return TryGetService<T>();
        }

        public object TryGetService(string name)
        {
            lock (Mutex)
            {
                _services.TryGetValue(name, out var service);
                return service;
            }
        }
        public T TryGetService<T>(string name)
        {
            lock (Mutex)
            {
                _services.TryGetValue(name, out var service);
                if (service is T value)
                {
                    return value;
                }
                return default(T);
            }
        }
        public T TryGetService<T>()
        {
            lock (Mutex)
            {
                foreach (var service in _services)
                {
                    if (service.Value is T value)
                    {
                        return value;
                    }
                }

                return default(T);
            }
        }
    }
}
