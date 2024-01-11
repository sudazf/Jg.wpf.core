namespace Jg.wpf.core.Service.Performance
{
    public abstract class ProfilerBase
    {

        public string Message { get; }

        protected ProfilerBase(string message)
        {
            Message = $"Start: {message}";
        }

        protected ProfilerBase(string format, params object[] args)
        {
            Message = string.Format(format, args);
        }
    }
}
