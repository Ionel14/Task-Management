using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskOrganizer.Model;
using TaskOrganizer.ViewModel;

namespace TaskOrganizer.View
{
    /// <summary>
    /// Interaction logic for ManageCategoryWindow.xaml
    /// </summary>
    public partial class ManageCategoryWindow : Window
    {
        public ManageCategoryWindow(ObservableCollection<Category> Categories)
        {
            InitializeComponent();
            DataContext = new ManageCategoriesVM(Categories);
        }
    }
}
