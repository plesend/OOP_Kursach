using System.Windows.Input;
using System;

namespace lab4_5
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _executeWithParam;
        private readonly Action _executeWithoutParam;
        private readonly Func<object, bool> _canExecuteWithParam;
        private readonly Func<bool> _canExecuteWithoutParam;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Конструктор для методов без параметров
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _executeWithoutParam = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithoutParam = canExecute;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _executeWithParam = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithParam = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteWithoutParam != null)
                return _canExecuteWithoutParam();

            if (_canExecuteWithParam != null)
                return _canExecuteWithParam(parameter);

            return true;
        }

        public void Execute(object parameter)
        {
            if (_executeWithoutParam != null)
                _executeWithoutParam();
            else
                _executeWithParam(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T>? _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || (parameter is T value && _canExecute(value));
        }

        public void Execute(object? parameter)
        {
            if (parameter is T value)
                _execute(value);
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }


}