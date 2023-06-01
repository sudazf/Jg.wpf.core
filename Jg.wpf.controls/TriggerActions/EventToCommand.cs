using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Jg.wpf.controls.TriggerActions
{
    public class EventToCommand : TriggerAction<FrameworkElement>
    {
        private object _commandParameterValue;
        private bool? _mustToggleValue;
        private ICommand _commandBackup;

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object),typeof(EventToCommand),new PropertyMetadata(null, OnCommandParameterChanged));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",typeof(ICommand),typeof(EventToCommand),new PropertyMetadata(OnCommandChanged));
        public static readonly DependencyProperty MustToggleIsEnabledProperty =DependencyProperty.Register("MustToggleIsEnabled", typeof(bool),typeof(EventToCommand),new PropertyMetadata(false, OnMustToggleIsEnabledChanged));

        #region Properties

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public object CommandParameterValue
        {
            get => (_commandParameterValue ?? CommandParameter);
            set
            {
                _commandParameterValue = value;
                EnableDisableElement();
            }
        }

        public bool MustToggleIsEnabled
        {
            get => (bool)GetValue(MustToggleIsEnabledProperty);
            set => SetValue(MustToggleIsEnabledProperty, value);
        }

        public bool MustToggleIsEnabledValue
        {
            get => _mustToggleValue ?? MustToggleIsEnabled;
            set
            {
                _mustToggleValue = value;
                EnableDisableElement();
            }
        }

        public bool PassEventArgsToCommand { get; set; }

        #endregion Properties

        #region Methods

        static EventToCommand()
        {

        }

        private static void OnMustToggleIsEnabledChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            if ((s is EventToCommand command) && (command.AssociatedObject != null))
            {
                command.EnableDisableElement();
            }
        }

        private static void OnCommandChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            if (s is EventToCommand command)
            {
                command.OnCommandChanged(e);
            }
        }

        private static void OnCommandParameterChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            if ((s is EventToCommand command) && (command.AssociatedObject != null))
            {
                command.EnableDisableElement();
            }
        }

        private bool AssociatedElementIsDisabled()
        {
            FrameworkElement associatedObject = GetAssociatedObject();
            return ((associatedObject != null) && !associatedObject.IsEnabled);
        }

        private void EnableDisableElement()
        {
            FrameworkElement associatedObject = GetAssociatedObject();
            if (associatedObject != null)
            {
                ICommand command = GetCommand();
                if (MustToggleIsEnabledValue && (command != null))
                {
                    associatedObject.IsEnabled = command.CanExecute(CommandParameterValue);
                }
            }
        }

        private FrameworkElement GetAssociatedObject()
        {
            return AssociatedObject;
        }

        private ICommand GetCommand()
        {
            return Command;
        }

        public void Invoke()
        {
            Invoke(null);
        }

        protected override void Invoke(object parameter)
        {
            if (!AssociatedElementIsDisabled())
            {
                ICommand command = GetCommand();
                object commandParameterValue = CommandParameterValue;
                if ((commandParameterValue == null) && PassEventArgsToCommand)
                {
                    commandParameterValue = parameter;
                }
                if ((command != null) && command.CanExecute(commandParameterValue))
                {
                    command.Execute(commandParameterValue);
                }
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            EnableDisableElement();
            FrameworkElement associatedObject = GetAssociatedObject();
            if (associatedObject != null)
            {
                associatedObject.Loaded += OnAssociatedObjectLoaded;
                associatedObject.Unloaded += OnAssociatedObjectUnloaded;
            }
        }

        private void OnAssociatedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            if (Command != null)
            {
                _commandBackup = Command;
                Command = null;
            }
            else
            {
                _commandBackup = null;
            }
        }

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            if (_commandBackup != null)
            {
                Command = _commandBackup;
            }
            _commandBackup = null;
        }

        protected override void OnDetaching()
        {
            Command = null;
            _commandBackup = null;
            FrameworkElement associatedObject = GetAssociatedObject();
            if (associatedObject != null)
            {
                associatedObject.Loaded -= OnAssociatedObjectLoaded;
                associatedObject.Unloaded -= OnAssociatedObjectUnloaded;
            }
            base.OnDetaching();
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            EnableDisableElement();
        }

        private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ((ICommand)e.OldValue).CanExecuteChanged -= OnCommandCanExecuteChanged;
            }
            ICommand newValue = (ICommand)e.NewValue;
            if (newValue != null)
            {
                newValue.CanExecuteChanged += OnCommandCanExecuteChanged;
            }
            EnableDisableElement();
        }

        #endregion Methods
    }
}
