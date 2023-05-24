using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskOrganizer.Model;

namespace TaskOrganizer.ViewModel
{   
    internal class ManageCategoriesVM : BaseVM
    {
        public ObservableCollection<Category> categories { get; set; }

        public ManageCategoriesVM(ObservableCollection<Category> Categories) 
        {
            categories = Categories;
        }

        private void AddCategory()
        {
            categories.Add(new Category(String.Empty));
        }

        private ICommand addCategoryCommand;
        public ICommand AddCategoryCommand
        {
            get
            {
                if (addCategoryCommand == null)
                {
                    addCategoryCommand = new RelayCommand<bool>(AddCategory);
                }
                return addCategoryCommand;
            }
        }private void DeleteCategory()
        {
            categories.Remove(SelectedItem);
        }

        private ICommand deleteCategoryCommand;
        public ICommand DeleteCategoryCommand
        {
            get
            {
                if (deleteCategoryCommand == null)
                {
                    deleteCategoryCommand = new RelayCommand<bool>(DeleteCategory);
                }
                return deleteCategoryCommand;
            }
        }

        private Category selectedItem;
        public Category SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }
    }
}
