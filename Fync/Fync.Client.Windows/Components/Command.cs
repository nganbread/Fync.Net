using System;
using System.Windows.Input;

namespace Fync.Client.Windows.Components
{
    internal class Command : ICommand
    {
        private readonly Action _command;
        private readonly Func<bool> _canExecute;
        private readonly Action<EventArgs> _canExecuteChanged;

        public Command(Action command)
            : this(command, null)
        {}

        public Command(Action command, Func<bool> canExecute)
            : this(command, canExecute, () => {})
        {}

        public Command(Action command, Func<bool> canExecute, Action canExecuteChanged)
            : this(command, canExecute, x => { canExecuteChanged(); })
        {}

        public Command(Action command, Func<bool> canExecute, Action<EventArgs> canExecuteChanged)
        {
            _command = command;
            _canExecute = canExecute;
            _canExecuteChanged = canExecuteChanged;

            CanExecuteChanged += OnCanExecuteChanged;
        }

        private void OnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            if (_canExecuteChanged != null) _canExecuteChanged(eventArgs);
        }

        public void Execute(object parameter)
        {
            if (_command != null) _command();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null
                ? true
                : _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}