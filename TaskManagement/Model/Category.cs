using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TaskOrganizer.ViewModel;

namespace TaskOrganizer.Model
{
    
    public class Category: BaseVM
    {
        public  Category() { }
        public Category(string s)
        {
            name = s;
        }

        
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("name");
            }
        }
    }
}
