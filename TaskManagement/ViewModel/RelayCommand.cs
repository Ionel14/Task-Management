using System;
using System.Windows.Input;

namespace TaskOrganizer.ViewModel
{
    internal class RelayCommand<T> : ICommand
    {
        private Action<T> commandTask;
        private Action commandTaskWithoutParams;

        public RelayCommand(Action<T> workToDo)
        {
            commandTask = workToDo;
        }
        public RelayCommand(Action workToDo)
        {
            commandTaskWithoutParams = workToDo;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (commandTaskWithoutParams != null)
            {
                commandTaskWithoutParams();
            }
            else
            {
                commandTask((T)parameter);
            }
        }      
    }
}
