using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskOrganizer.Model;
using TaskOrganizer.View;

namespace TaskOrganizer.ViewModel
{
    internal class AddToDoListVM : BaseVM
    {
        private string[] iconsPaths;
        private int index = 0;
        private ToDoList tdl;
        public AddToDoListVM(ToDoList newTdl) 
        {
            iconsPaths = Directory.GetFiles("../../Icons", "*.png");
            tdl = newTdl;
            tdl.ImagePath = iconsPaths[index];
        }      
        public string Name
        {
            get
            {
                return tdl.Name;
            }
            set 
            { 
                tdl.Name = value;
                NotifyPropertyChanged("Name");
            }
        }
        public string ImagePath
        {
            get
            {
                return tdl.ImagePath;
            }
            set
            {
                tdl.ImagePath = value;
                NotifyPropertyChanged("ImagePath");
            }
        }

        private void onLeftButtonClick()
        {
            if (index == 0) 
            { 
                index = iconsPaths.Length - 1;
            }
            else
            {
                index--;
            }
            ImagePath = iconsPaths[index];
        }

        private ICommand leftPhCommand;
        public ICommand LeftPhCommand
        {
            get
            {
                if (leftPhCommand == null)
                {
                    leftPhCommand = new RelayCommand<bool>(onLeftButtonClick);
                }
                return leftPhCommand;
            }
        }

       
        private void onRightButtonClick()
        {
            if (index == iconsPaths.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            ImagePath = iconsPaths[index];
        }
        
        private ICommand rightPhCommand;
        public ICommand RightPhCommand
        {
            get
            {
                if (rightPhCommand == null)
                {
                    rightPhCommand = new RelayCommand<bool>(onRightButtonClick);
                }
                return rightPhCommand;
            }
        }
    }
}
