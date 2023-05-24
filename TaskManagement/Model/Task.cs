using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using TaskOrganizer.ViewModel;

namespace TaskOrganizer.Model
{
    
    public class Task : BaseVM
    {
        public Task() 
        { 
            description = "some description";
            deadline = DateTime.Today;
            priority = Priorities[0];
            status = Statuses[0];
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
        public static ObservableCollection<string> Statuses { get; set; } = new ObservableCollection<String> { "Created", "InProgress", "Done" } ;
        public static ObservableCollection<string> Priorities { get; set; } = new ObservableCollection<String> { "High", "Medium", "Low" };
        
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                NotifyPropertyChanged("Description");
            }
        }

        
        private String status;
        public String Status 
        {
            get 
            { 
                return status; 
            }
            set
            {
                status = value;
                NotifyPropertyChanged("Status");
            }
        }

        
        private String priority;
        public String Priority
        {
            get 
            {             
                return priority;
            }
            set 
            {
                priority = value;
                NotifyPropertyChanged("Priority");
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

        
        private DateTime taskDoneDate;
        public DateTime TaskDoneDate
        {
            get
            {
                return taskDoneDate;
            }
            set
            {
                if (taskDoneDate != DateTime.MinValue)
                {
                    BgkColor = new SolidColorBrush(Colors.BlueViolet);
                }
                taskDoneDate = value;
                NotifyPropertyChanged("TaskDoneDate");
            }
        }

        
        private Category category;
        public Category Category
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
                NotifyPropertyChanged("Category");
            }
        }

        private void setTaskDone()
        {
            taskDoneDate = DateTime.Now;
            Status = Statuses[2];
            BgkColor = new SolidColorBrush(Colors.BlueViolet);
        }

        private ICommand markDone;
        public ICommand MarkDone
        {
            get
            {
                if (markDone == null)
                {
                    markDone = new RelayCommand<bool>(setTaskDone);
                }
                return markDone;
            }
        }

        [XmlIgnore]
        private Brush bgkColor;
        [XmlIgnore]
        public Brush BgkColor
        {
            get
            {
                return bgkColor;
            }
            set
            {
                bgkColor = value;
                NotifyPropertyChanged("bgkColor");
            }
        }

    }
}
