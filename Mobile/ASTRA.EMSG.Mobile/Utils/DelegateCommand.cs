using System;
using System.Windows.Input;

namespace ASTRA.EMSG.Mobile.Utils
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Predicate<T> canExecute;
        private readonly Action<T> method;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<T> method)
            : this(method, null)
        {
        }

        public DelegateCommand(Action<T> method, Predicate<T> canExecute)
        {
            this.method = method;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            method.Invoke((T)parameter);
        }

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            var canExecuteChanged = CanExecuteChanged;

            if (canExecuteChanged != null)
                canExecuteChanged(this, e);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }
    }

    public class DelegateCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action method;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action method)
            : this(method, null)
        {
        }

        public DelegateCommand(Action method, Func<bool> canExecute)
        {
            this.method = method;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object parameter)
        {
            method.Invoke();
        }

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            var canExecuteChanged = CanExecuteChanged;

            if (canExecuteChanged != null)
                canExecuteChanged(this, e);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }
    }
}
