namespace Demo.Common
{
    using System;
    using System.Windows.Input;

    public class DelegateCommand : ICommand
    {
        private Predicate<object> _canExecuteHandler;
        private Action<object> _executeHandler;
        private bool _canExecuteCache;
        bool _isRaising = false;

        public DelegateCommand(Action<object> executeHandler)
        {
            _executeHandler = executeHandler;
        }

        public DelegateCommand(Predicate<object> canExecuteHandler, Action<object> executeHandler)
        {
            _canExecuteHandler = canExecuteHandler;
            _executeHandler = executeHandler;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteHandler == null) return true;
            var canExecute = _canExecuteHandler(parameter);
            if (canExecute != _canExecuteCache)
            {
                _canExecuteCache = canExecute;
                if (CanExecuteChanged != null && _isRaising == false)
                    CanExecuteChanged(this, EventArgs.Empty);
            }
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _executeHandler(parameter);
        }
        
        public void RaiseCanExecuteChanged()
        {
            _isRaising = true;
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
            _isRaising = false;
        }
    }
}
