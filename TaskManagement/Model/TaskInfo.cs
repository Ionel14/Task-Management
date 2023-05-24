using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskOrganizer.ViewModel;

namespace TaskOrganizer.Model
{
    public class TaskInfo : BaseVM
    {
        public TaskInfo(string name, string loc, string iconP) 
        {
            taskName = name;
            location = loc;
            parentIconPath = iconP;
        }

        private string taskName;
        public string TaskName 
        { 
            get
            {
                return taskName;
            }
            set
            {
                taskName = value;
                NotifyPropertyChanged("TaskName");
            }
        }
        
        public string location;
        public string Location 
        { 
            get
            {
                return location;
            }
            set
            {
                location = value;
                NotifyPropertyChanged("location");
            }
        }
 
        private string parentIconPath;
        public string ParentIconPath
        { 
            get
            {
                return parentIconPath;
            }
            set
            {
                parentIconPath = value;
                NotifyPropertyChanged("ParentIconPath");
            }
        }
    }
}
