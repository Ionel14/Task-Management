using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using TaskOrganizer.Model;
using TaskOrganizer.View;

namespace TaskOrganizer.ViewModel
{
    internal class StartPageVM : BaseVM
    {
        private ObservableCollection<Database> _databases = new ObservableCollection<Database>();
        public ObservableCollection<Database> databases
        {
            get { return _databases; }
            set
            {
                _databases = value;
                NotifyPropertyChanged("databases");
            }
        }

        string[] xmlFiles = Directory.GetFiles("../../Databases", "*.xml");
        private readonly StartPage thisPage;
        public StartPageVM(StartPage startPage)
        {
            thisPage = startPage;
            foreach (string fileName in xmlFiles)
            {
                databases.Add(new Database(Path.GetFileNameWithoutExtension(fileName)));
            }
        }

        private Database selectedDatabase;
        public Database SelectedDatabase
        {
            get { return selectedDatabase; }
            set
            {
                selectedDatabase = value;
                NotifyPropertyChanged("SelectedDatabase");
            }
        }

        private void addNewDatabase()
        {
            Database newDatabase = new Database();
            newDatabase.IsReadOnly = false;
            newDatabase.NameSetVis = Visibility.Visible;
            databases.Add(newDatabase);
        }

        private ICommand addNewDatabaseCommand;
        public ICommand AddNewDatabaseCommand
        {
            get
            {
                if (addNewDatabaseCommand == null)
                {
                    addNewDatabaseCommand = new RelayCommand<bool>(addNewDatabase);
                }
                return addNewDatabaseCommand;
            }
        }

        private void OpenDatabase()
        {
            if (SelectedDatabase == null)
            {
                return;
            }
            const string filePath = @"..\..\DataBases\";
            TreeViewVM treeViewVM;
            if (File.Exists(filePath + SelectedDatabase.Name + ".xml"))
            {
                XmlSerializer ser = new XmlSerializer(typeof(TreeViewVM));
                FileStream reader = File.OpenRead(filePath + SelectedDatabase.Name + ".xml");
                treeViewVM = (TreeViewVM)ser.Deserialize(reader);
                reader.Dispose();
            }
            else
            {
                treeViewVM = new TreeViewVM();
            }

            treeViewVM.databaseName = SelectedDatabase.Name;
            MainWindow mainWindow = new MainWindow(treeViewVM);
            mainWindow.Show();
            thisPage.Close();
        }

        private ICommand openDatabaseCommand;
        public ICommand OpenDatabaseCommand
        {
            get
            {
                if (openDatabaseCommand == null)
                {
                    openDatabaseCommand = new RelayCommand<bool>(OpenDatabase);
                }
                return openDatabaseCommand;
            }
        }

    }
}
