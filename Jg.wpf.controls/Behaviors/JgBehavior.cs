using System.Windows;
using System.Windows.Interactivity;

namespace Jg.wpf.controls.Behaviors
{
    public abstract class JgBehavior<T> : Behavior<T> where T : FrameworkElement
    {
        private bool _isCleanedUp;

        protected sealed override void OnAttached()
        {
            if (CheckCanRegister())
            {
                if (AssociatedObject != null)
                {
                    AssociatedObject.Loaded += OnAssociatedObjectLoaded;
                    AssociatedObject.Unloaded += OnAssociatedObjectUnLoaded;
                }
                OnAssociatedObjectRegistered();
            }
        }
        protected virtual bool CheckCanRegister()
        {
            return AssociatedObject != null;
        }
        protected virtual void OnAssociatedObjectRegistered()
        {

        }
        protected virtual void OnAssociatedObjectLoaded(object sender)
        {
            OnAssociatedObjectLoaded();
        }
        protected virtual void OnAssociatedObjectLoaded()
        {

        }
        protected sealed override void OnDetaching()
        {
            CleanUp();
        }
        protected virtual void OnCleanUp()
        {

        }

        private void OnAssociatedObjectUnLoaded(object sender, RoutedEventArgs e)
        {
            CleanUp();
        }
        private void CleanUp()
        {
            if (!_isCleanedUp)
            {
                _isCleanedUp = true;
                OnCleanUp();
                if (CheckCanRegister())
                {
                    if (AssociatedObject != null)
                    {
                        AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
                        AssociatedObject.Unloaded -= OnAssociatedObjectUnLoaded;
                    }
                }
            }
        }
        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            OnAssociatedObjectLoaded(sender);
        }
    }

    public enum JgOrientation
    {
        Horizontal,
        Vertical,
    }
}
