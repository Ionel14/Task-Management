using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddToDoList.xaml
    /// </summary>
    public partial class AddToDoList : Window
    {
        HashSet<String> tdluri;
        public AddToDoList(ToDoList newTdl, HashSet<String> tdluri)
        {
            InitializeComponent();
            DataContext = new AddToDoListVM(newTdl);
            this.tdluri = tdluri;
        }

        private const int minLengthName = 2;
        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (usernameBox.Text.Length < minLengthName)
            {
                MessageBox.Show("Name is too short");
                return;
            }
            if (tdluri.Contains(usernameBox.Text))
            {
                MessageBox.Show("Already exists a to do list with this name");
                return;
            }
            Close();
        }
    }
}
