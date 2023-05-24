using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskOrganizer.Business;
using TaskOrganizer.Model;
using TaskOrganizer.View;
using Task = TaskOrganizer.Model.Task;

namespace TaskOrganizer.ViewModel
{
    internal class ViewTasksVM : BaseVM
    {
        public ObservableCollection<Task> tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                NotifyPropertyChanged("tasks");
            }
        }
        private ObservableCollection<Task> _tasks = new ObservableCollection<Task>();
        private TasksFilter tasksManagement = new TasksFilter();
        public ObservableCollection<Category> Categories { get; set; }

        public ViewTasksVM(ObservableCollection<Category> categories)
        {
            Categories = categories;
            tasks = tasksManagement.getAllTasks();
        }

        private void sortByDeadline()
        {
            tasks = tasksManagement.sortByDeadline();
        }

        private ICommand sortByDeadlineCommand;
        public ICommand SortByDeadlineCommand
        {
            get
            {
                if (sortByDeadlineCommand == null)
                {
                    sortByDeadlineCommand = new RelayCommand<bool>(sortByDeadline);
                }
                return sortByDeadlineCommand;
            }
        }

        private void sortByPriority()
        {
            tasks = tasksManagement.sortByPriority();
        }

        private ICommand sortByPriorityCommand;
        public ICommand SortByPriorityCommand
        {
            get
            {
                if (sortByPriorityCommand == null)
                {
                    sortByPriorityCommand = new RelayCommand<bool>(sortByPriority);
                }
                return sortByPriorityCommand;
            }
        }

        private void resetTasks()
        {
            _tasks.Clear();
            tasks = tasksManagement.getAllTasks();
        }

        private ICommand resetTasksCommand;
        public ICommand ResetTasksCommand
        {
            get
            {
                if (resetTasksCommand == null)
                {
                    resetTasksCommand = new RelayCommand<bool>(resetTasks);
                }
                return resetTasksCommand;
            }
        }

        private Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                tasks = tasksManagement.FilterByCategory(value);
                NotifyPropertyChanged("SelectedCategory");
            }
        }

        private void GetDoneTasks()
        {
            tasks = tasksManagement.DoneStatusTasks();
        }

        private ICommand doneTasksCommand;
        public ICommand DoneTasksCommand
        {
            get
            {
                if (doneTasksCommand == null)
                {
                    doneTasksCommand = new RelayCommand<bool>(GetDoneTasks);
                }
                return doneTasksCommand;
            }
        }

        private void GetOverdueTasks()
        {
            tasks = tasksManagement.OverdueTasks();
        }

        private ICommand overdueTasksCommand;
        public ICommand OverdueTasksCommand
        {
            get
            {
                if (overdueTasksCommand == null)
                {
                    overdueTasksCommand = new RelayCommand<bool>(GetOverdueTasks);
                }
                return overdueTasksCommand;
            }
        }

        private void GetShouldBeDoneTasks()
        {
            tasks = tasksManagement.ShouldBeFinishedTasks();
        }

        private ICommand shouldBeDoneTasksCommand;
        public ICommand ShouldBeDoneTasksCommand
        {
            get
            {
                if (shouldBeDoneTasksCommand == null)
                {
                    shouldBeDoneTasksCommand = new RelayCommand<bool>(GetShouldBeDoneTasks);
                }
                return shouldBeDoneTasksCommand;
            }
        }

        private void GetUnfinishedTasks()
        {
            tasks = tasksManagement.UnfinishedTasks();
        }

        private ICommand unfinishedTasksCommand;
        public ICommand UnfinishedTasksCommand
        {
            get
            {
                if (unfinishedTasksCommand == null)
                {
                    unfinishedTasksCommand = new RelayCommand<bool>(GetUnfinishedTasks);
                }
                return unfinishedTasksCommand;
            }
        }


    }
}
