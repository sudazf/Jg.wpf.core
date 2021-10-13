using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Input;
using Jg.wpf.core.Log;

namespace Jg.wpf.core.Command
{
    public abstract class CommandBase : ICommand
    {
        private string _description;
        private bool _isEnabled = true;
        private bool _isVisible = true;
        private bool _isChecked;
        private bool _canExecuted;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CanExecuteChanged;
        public event EventHandler Executed;

        public string Id { get; }

        public Action<object> ExecuteAction { get; protected set; }
        public Func<object, bool> CanExecuteAction { get; protected set; }

        public bool CanExecuted
        {
            get => _canExecuted;
            set
            {
                _canExecuted = value;
                RaisePropertyChanged(() => CanExecuted);
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                RaisePropertyChanged(() => IsVisible);
            }
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        protected CommandBase(string id, Action<object> executeMethod, Func<object, bool> canExecuteMethod = null, string description = null) : this(id, description)
        {
            ExecuteAction = executeMethod;
            CanExecuteAction = canExecuteMethod;
        }
        protected CommandBase(string id, string description = null)
        {
            Id = id;
            _description = description;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = (propertyExpression.Body as MemberExpression)?.Member.Name;
            RaisePropertyChanged(propertyName);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                OnExecuting(parameter);
                try
                {
                    var executeMethod = ExecuteAction;
                    executeMethod?.Invoke(parameter);
                    OnExecuted(parameter);
                }
                catch (Exception e)
                {
                    Logger.WriteLineError(e.Message);
                }
            }
        }
        public virtual bool CanExecute(object parameter)
        {
            var canExecute = true;
            var canExecuteMethod = CanExecuteAction;
            if (canExecuteMethod != null)
            {
                canExecute = canExecuteMethod(parameter);
            }

            CanExecuted = canExecute;
            return canExecute;
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnExecuting(object parameter)
        {

        }
        protected virtual void OnExecuted(object parameter)
        {
            EventHandler handler = Executed;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
