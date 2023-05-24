using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TaskOrganizer.ViewModel;

namespace TaskOrganizer.Model
{
    
    public class ToDoList : BaseVM
    {
        public ToDoList()
        {
            subList = new ObservableCollection<ToDoList>();
            Tasks = new ObservableCollection<Task>();
        }

        
        public ObservableCollection<ToDoList> subList { get; set; }
        
        public ObservableCollection<Task> Tasks { get; set; }

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

        private string imagePath;
        
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
                NotifyPropertyChanged("ImagePath");
            }
        }
    }
}
