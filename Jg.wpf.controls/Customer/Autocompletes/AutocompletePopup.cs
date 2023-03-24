using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Jg.wpf.controls.Customer.Autocompletes
{
    public class AutocompletePopup : Popup
    {
        private Window _window;

        public AutocompletePopup()
        {
            _window = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        protected virtual void LoadedHandler(object sender, RoutedEventArgs args)
        {
            _window = Window.GetWindow(this);

            if (_window != null)
            {
                _window.SizeChanged += WindowSizeChangedHandler;
                _window.LocationChanged += WindowLocationChanged;
            }
        }

        protected virtual void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            if (_window != null)
            {
                _window.SizeChanged -= WindowSizeChangedHandler;
                _window.LocationChanged -= WindowLocationChanged;
            }
        }

        private void WindowSizeChangedHandler(object sender, SizeChangedEventArgs args)
        {
            UpdatePosition();
        }

        private void WindowLocationChanged(object sender, EventArgs args)
        {
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            if (IsOpen)
            {
                // change the offset to trigger an update of the Popup user interface
                double offset = VerticalOffset;
                VerticalOffset = offset + 1;
                VerticalOffset = offset;
            }
        }
    }
}
