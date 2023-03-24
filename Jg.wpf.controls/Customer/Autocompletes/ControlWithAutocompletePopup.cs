using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jg.wpf.controls.Customer.Autocompletes
{
    public abstract class ControlWithAutocompletePopup : Control
    {
        protected Window _window;
        protected AutocompletePopup _popup;

        /// <summary>
        /// Creates a new <see cref="ControlWithAutocompletePopup" />.
        /// </summary>
        public ControlWithAutocompletePopup() : base()
        {
            _window = null;
            _popup = null;

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

        protected virtual void WindowSizeChangedHandler(object sender, SizeChangedEventArgs args)
        {
            ClearFocus();
        }

        protected virtual void WindowLocationChanged(object sender, EventArgs args)
        {
            ClearFocus();
        }

        protected void ClearFocus()
        {
            if (IsKeyboardFocusWithin)
            {
                Keyboard.ClearFocus();
            }
        }

        public void UpdatePopup()
        {
            _popup?.UpdatePosition();
        }
    }
}
