using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Jg.wpf.controls.Behaviors
{
    public abstract class JgBehavior<T> : Behavior<T> where T : FrameworkElement
    {
        private bool _isCleanedUp;

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded += OnAssociatedObjectLoaded;
                AssociatedObject.Unloaded += OnAssociatedObjectUnLoaded;
            }
        }

        protected virtual void OnAssociatedObjectLoaded()
        {

        }

        protected virtual void OnAssociatedObjectUnloaded()
        {

        }

        protected virtual void OnCleanUp()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
                AssociatedObject.Unloaded -= OnAssociatedObjectUnLoaded;
            }
        }

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            OnAssociatedObjectLoaded();
        }

        private void OnAssociatedObjectUnLoaded(object sender, RoutedEventArgs e)
        {
            OnAssociatedObjectUnloaded();
        }

        protected override void OnDetaching()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            if (!_isCleanedUp)
            {
                _isCleanedUp = true;
                OnCleanUp();
            }
        }
    }

    public enum JgOrientation
    {
        Horizontal,
        Vertical,
    }
}
