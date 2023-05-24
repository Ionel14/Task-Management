using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskOrganizer.ViewModel;

namespace TaskOrganizer.Model
{
    internal class Database : BaseVM
    {
        public Database(string name)
        {
            Name = name;
        }

        public Database(){}

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

        private Visibility nameSetVis = Visibility.Hidden;
        public Visibility NameSetVis
        {
            get
            {
                return nameSetVis;
            }
            set
            {
                nameSetVis = value;
                NotifyPropertyChanged("NameSetVis");
            }
        }

        private bool isReadOnly = true;
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly; 
            }
            set 
            {
                isReadOnly = value;
                NotifyPropertyChanged("IsReadOnly");
            }
        }

        private void setName()
        {
            IsReadOnly = true;
            NameSetVis = Visibility.Hidden;
        }

        private ICommand setNameCommand;
        public ICommand SetNameCommand
        {
            get
            {
                if (setNameCommand == null)
                {
                    setNameCommand = new RelayCommand<bool>(setName);
                }
                return setNameCommand;
            }
        }
    }
}
