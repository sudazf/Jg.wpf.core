using System;

namespace Jg.wpf.core.Service.FileService
{
    public interface IXmlFileService
    {
        T Load<T>(string path);
        T1 Load<T1>(string path, Type[] types);

        void Save<T>(T instance, string path);
        void Save<T>(T obj, string fullPath, Type[] types);
    }
    public interface IXmlFileService<T> : ISave<T>, ILoad<T> where T : class, new()
    {

    }
    public interface ILoad<out T> where T : class
    {
        T Load(string path);
    }

    public interface ISave<in T> where T : class
    {
        bool Save(T obj, string path);
    }
}
