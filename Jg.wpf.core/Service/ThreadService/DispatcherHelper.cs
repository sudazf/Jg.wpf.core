using System.Windows.Threading;

namespace Jg.wpf.core.Service.ThreadService
{
    public static class DispatcherHelper
    {
        public static void DoEvents()
        {
            var nestedFrame = new DispatcherFrame();
            var exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrame), nestedFrame);

            Dispatcher.PushFrame(nestedFrame);
            if (exitOperation.Status != DispatcherOperationStatus.Completed)
            {
                exitOperation.Abort();
            }
        }

        private static object ExitFrame(object arg)
        {
            ((DispatcherFrame)arg).Continue = false;
            return null;
        }
    }
}
