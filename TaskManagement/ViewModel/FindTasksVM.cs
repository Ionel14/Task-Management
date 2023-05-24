using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskOrganizer.Business;
using TaskOrganizer.Model;

namespace TaskOrganizer.ViewModel
{
    internal class FindTasksVM : BaseVM
    {
        public ObservableCollection<TaskInfo> tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                NotifyPropertyChanged("tasks");
            }
        }
        private ObservableCollection<TaskInfo> _tasks = new ObservableCollection<TaskInfo>();

        private TasksFilter taskFilterManag = new TasksFilter();

        public FindTasksVM()
        {
            deadline = DateTime.MinValue;
        }

        private ICommand findTasksCommand;
        public ICommand FindTasksCommand
        {
            get
            {
                if (findTasksCommand == null)
                {
                    findTasksCommand = new RelayCommand<bool>(findTasks);
                }   
                return findTasksCommand;
            }
            set
            {
                findTasksCommand = value;
            }
        }

        private void findTasks()
        {
            tasks = taskFilterManag.Find(name, deadline);
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }
        
        private DateTime deadline;
        public DateTime Deadline
        {
            get
            {
                return deadline;
            }
            set
            {
                deadline = value;
                NotifyPropertyChanged("Deadline");
            }
        }


    }
}
