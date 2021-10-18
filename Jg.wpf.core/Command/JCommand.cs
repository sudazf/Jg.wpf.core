using System;

namespace Jg.wpf.core.Command
{
    public class JCommand : CommandBase
    {
        public JCommand(string id, Action<object> executeMethod, Func<object, bool> canExecuteMethod = null, string description = null) : base(id, executeMethod, canExecuteMethod, description)
        {
        }

        public JCommand(string id, string description = null) : base(id, description)
        {
        }
    }
}
