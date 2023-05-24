using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;
using TaskOrganizer.Business;
using TaskOrganizer.Model;
using TaskOrganizer.View;
using Task = TaskOrganizer.Model.Task;


namespace TaskOrganizer.ViewModel
{
    
    public class TreeViewVM : BaseVM
    {
        
        private HashSet<String> tdluri = new HashSet<String>();
        
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public TreeViewVM()
        {
          
        }

        public string databaseName { get; set; }
        private void refreshStatistics()
        {
            TaskDueToday = 0;
            TaskDueTomorrow = 0;
            TaskDone = 0;
            TaskOverdue = 0;
            TaskToBeDone = 0;

            Stack<ToDoList> toDoLists = new Stack<ToDoList>();
            foreach (ToDoList tdl in rootList)
            {
                toDoLists.Push(tdl);
            }

            ObservableCollection<Task> tasks;
            ToDoList currentTdl;
            while (toDoLists.Count != 0)
            {
                currentTdl = toDoLists.Pop();
                tasks = currentTdl.Tasks;

                foreach (Task task in tasks)
                {
                    updateStatistics(task);
                }

                foreach (ToDoList tdl in currentTdl.subList)
                {
                    toDoLists.Push(tdl);
                }
            }
        }

        private void updateStatistics(Task task)
        {
            if (task.TaskDoneDate != DateTime.MinValue && task.Deadline < task.TaskDoneDate)
            {
                TaskOverdue++;
            }
            else if (task.TaskDoneDate == DateTime.MinValue)
            {
                TaskToBeDone++;
                if (task.Deadline.Date == DateTime.Today)
                {
                    TaskDueToday++;
                }
                else if (task.Deadline.Date == DateTime.Today.AddDays(1))
                {
                    TaskDueTomorrow++;
                }
            }
            else
            {
                TaskDone++;
            }
        }

        
        public ObservableCollection<ToDoList> rootList { get; set; } = new ObservableCollection<ToDoList> ();

        private bool autoSelect = false;

        
        private int taskDueToday;
        public int TaskDueToday
        {
            get { return taskDueToday; }
            set
            {
                taskDueToday = value;
                NotifyPropertyChanged("TaskDueToday");
            }
        }
        
        private int taskDueTomorrow;
        public int TaskDueTomorrow
        {
            get { return taskDueTomorrow; }
            set
            {
                taskDueTomorrow = value;
                NotifyPropertyChanged("TaskDueTomorrow");
            }
        }

        
        private int taskOverdue;
        public int TaskOverdue
        {
            get { return taskOverdue; }
            set
            {
                taskOverdue = value;
                NotifyPropertyChanged("TaskOverdue");
            }
        }

        
        private int taskDone;
        public int TaskDone
        {
            get { return taskDone; }
            set
            {
                taskDone = value;
                NotifyPropertyChanged("TaskDone");
            }
        }
        
        private int taskToBeDone;
        public int TaskToBeDone
        {
            get { return taskToBeDone; }
            set
            {
                taskToBeDone = value;
                NotifyPropertyChanged("TaskToBeDone");
            }
        }

        
        private ToDoList selectedItem;
        public ToDoList SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (rootSelectVis == Visibility.Visible)
                {
                    if (!autoSelect)
                    {
                        autoSelect = true;
                        return;
                    }
                    value.subList.Add(selectedItem);
                    RootSelectVis = Visibility.Hidden;
                    autoSelect = false;
                }
                selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }

        private void GetSelectedTdl(ToDoList tdl)
        {
            SelectedItem = tdl;
        }

        [XmlIgnore]
        private ICommand changeTdlCommand;
        [XmlIgnore]
        public ICommand ChangeTdlCommand
        {
            get
            {
                if (changeTdlCommand == null)
                {
                    changeTdlCommand = new RelayCommand<ToDoList>(GetSelectedTdl);
                }
                return changeTdlCommand;
            }
            set
            {
                changeTdlCommand = value;
            }
        }

        private void GoToAddTdlWindow()
        {
            ToDoList newTdl = new ToDoList();
            AddToDoList addToDoList = new AddToDoList(newTdl, tdluri);
            addToDoList.ShowDialog();
            tdluri.Add(newTdl.Name);
            if (selectedItem != null)
            {
                selectedItem.subList.Add(newTdl);
            }
            else
            {
                rootList.Add(newTdl);
            }
            NotifyPropertyChanged("rootList");
        }

        [XmlIgnore]
        private ICommand addNewTdlCommand;
        [XmlIgnore]
        public ICommand AddNewTdlCommand
        {
            get
            {
                if (addNewTdlCommand == null)
                {
                    addNewTdlCommand = new RelayCommand<bool>(GoToAddTdlWindow);
                }
                return addNewTdlCommand;
            }
        }

        
        private Task selectedTask;
        public Task SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                NotifyPropertyChanged("SelectedTask");
            }
        }

        private void addTask()
        {
            if (SelectedItem != null)
            {
                SelectedItem.Tasks.Add(new Task());
                taskToBeDone++;
            }
            
        }

        [XmlIgnore]
        private ICommand addNewTask;
        [XmlIgnore]
        public ICommand AddNewTask
        {
            get
            {
                if (addNewTask == null)
                {
                    addNewTask = new RelayCommand<bool>(addTask);
                }
                return addNewTask;
            }
        }

        private void delTask(Task task)
        {
            if (task.TaskDoneDate != DateTime.MinValue && task.Deadline < task.TaskDoneDate)
            {
                TaskOverdue--;
            }
            else if (task.TaskDoneDate == DateTime.MinValue)
            {
                TaskToBeDone--;
                if (task.Deadline.Date == DateTime.Today)
                {
                    TaskDueToday--;
                }
                else if (task.Deadline.Date == DateTime.Today.AddDays(1))
                {
                    TaskDueTomorrow--;
                }
            }
            else
            {
                TaskDone--;
            }
            selectedItem.Tasks.Remove(task);
        }

        [XmlIgnore]
        private ICommand deleteTask;
        [XmlIgnore]
        public ICommand DeleteTask
        {
            get
            {
                if (deleteTask == null)
                {
                    deleteTask = new RelayCommand<Task>(delTask);
                }
                return deleteTask;
            }
        }

        private void moveTask(String adder)
        {
            if (SelectedTask != null)
            {
                int index = SelectedItem.Tasks.IndexOf(SelectedTask);
                int newPos = index + int.Parse(adder);
                if (newPos < SelectedItem.Tasks.Count && newPos >= 0)
                {
                    SelectedItem.Tasks.Move(index, newPos);
                }
            }
        }

        [XmlIgnore]
        private ICommand moveTaskCommand;
        [XmlIgnore]
        public ICommand MoveTaskCommand
        {
            get
            {
                if (moveTaskCommand == null)
                {
                    moveTaskCommand = new RelayCommand<String>(moveTask);
                }
                return moveTaskCommand;
            }
        }

        private ObservableCollection<ToDoList> findParentListOfTdl(ToDoList toDoList)
        {
            Stack<ToDoList> toDoListsToSearch = new Stack<ToDoList>();
            foreach (var tdl in rootList)
            {
                if (tdl == toDoList)
                {
                    return rootList;
                }
                toDoListsToSearch.Push(tdl);
            }

            while (toDoListsToSearch.Count > 0)
            {
                ToDoList tdl = toDoListsToSearch.Pop();

                foreach (var tdl1 in tdl.subList)
                {
                    if (tdl1 == toDoList)
                    {
                        return tdl.subList;
                    }
                    toDoListsToSearch.Push(tdl1);
                }
            }

            return null;
        }

        private void moveTdl(String adder)
        {
            if (SelectedItem != null)
            {
                ObservableCollection<ToDoList> parent = findParentListOfTdl(SelectedItem);
                int index = parent.IndexOf(SelectedItem);
                int newPos = index + int.Parse(adder);
                if (newPos < parent.Count && newPos >= 0)
                {
                    parent.Move(index, newPos);
                }
            }
        }

        [XmlIgnore]
        private ICommand moveTdlCommand;
        [XmlIgnore]
        public ICommand MoveTdlCommand
        {
            get
            {
                if (moveTdlCommand == null)
                {
                    moveTdlCommand = new RelayCommand<String>(moveTdl);
                }
                return moveTdlCommand;
            }
        }
        
        private Visibility rootSelectVis = Visibility.Hidden;
        public Visibility RootSelectVis
        {
            get
            {
                return rootSelectVis;
            }
            set
            {
                rootSelectVis = value;
                NotifyPropertyChanged("RootSelectVis");
            }
        }
        private void changeRootTdl()
        {
            if (SelectedItem != null)
            {
                RootSelectVis = Visibility.Visible;
                MessageBox.Show("Select new parent for Tdl or click the root button to make it root tdl");
                ObservableCollection<ToDoList> parent = findParentListOfTdl(SelectedItem);
                parent.Remove(SelectedItem);
            }
        }

        [XmlIgnore]
        private ICommand changeRootTdlCommand;
        [XmlIgnore]
        public ICommand ChangeRootTdlCommand
        {
            get
            {
                if (changeRootTdlCommand == null)
                {
                    changeRootTdlCommand = new RelayCommand<bool>(changeRootTdl);
                }
                return changeRootTdlCommand;
            }
        }

        private void setRootAsParent()
        {
            rootList.Add(SelectedItem);
            RootSelectVis = Visibility.Hidden;
        }

        [XmlIgnore]
        private ICommand setRootListAsParentCommand;
        [XmlIgnore]
        public ICommand SetRootListAsParentCommand
        {
            get
            {
                if (setRootListAsParentCommand == null)
                {
                    setRootListAsParentCommand = new RelayCommand<bool>(setRootAsParent);
                }
                return setRootListAsParentCommand;
            }
        }

        private void GoToEditTdlWindow()
        {
            if (SelectedItem != null)
            {
                tdluri.Remove(SelectedItem.Name);
                AddToDoList addToDoList = new AddToDoList(SelectedItem, tdluri);
                addToDoList.ShowDialog();
                tdluri.Add(SelectedItem.Name);
                NotifyPropertyChanged("rootList");
            }
        }

        [XmlIgnore]
        private ICommand editTdlCommand;
        [XmlIgnore]
        public ICommand EditTdlCommand
        {
            get
            {
                if (editTdlCommand == null)
                {
                    editTdlCommand = new RelayCommand<bool>(GoToEditTdlWindow);
                }
                return editTdlCommand;
            }
        }

        private void deleteTdl()
        {
            if (SelectedItem != null)
            {
                Stack<ToDoList> toDoLists = new Stack<ToDoList>();
                toDoLists.Push(SelectedItem);

                ObservableCollection<Task> tasks;
                while (toDoLists.Count != 0)
                {
                    tasks = toDoLists.Pop().Tasks;

                    while (tasks.Count != 0)
                    {
                        delTask(tasks[0]);
                    }
                }

                ObservableCollection<ToDoList> parent = findParentListOfTdl(SelectedItem);
                parent.Remove(SelectedItem);
            }
        }

        [XmlIgnore]
        private ICommand deleteTdlCommand;
        [XmlIgnore]
        public ICommand DeleteTdlCommand
        {
            get
            {
                if (deleteTdlCommand == null)
                {
                    deleteTdlCommand = new RelayCommand<bool>(deleteTdl);
                }
                return deleteTdlCommand;
            }
        }
        
        private void ManageCategories()
        {
            ManageCategoryWindow categoryWindow = new ManageCategoryWindow(Categories);
            categoryWindow.Show();
        }

        [XmlIgnore]
        private ICommand manageCategoriesCommand;
        [XmlIgnore]
        public ICommand ManageCategoriesCommand
        {
            get
            {
                if (manageCategoriesCommand == null)
                {
                    manageCategoriesCommand = new RelayCommand<bool>(ManageCategories);
                }
                return manageCategoriesCommand;
            }
        }
        
        private void FindTask()
        {
            TasksFilter.rootList = rootList;
            FindTasks findTasks = new FindTasks();
            findTasks.Show();
        }


        [XmlIgnore]
        private ICommand findTaskCommand;
        [XmlIgnore]
        public ICommand FindTaskCommand
        {
            get
            {
                if (findTaskCommand == null)
                {
                    findTaskCommand = new RelayCommand<bool>(FindTask);
                }
                return findTaskCommand;
            }
        }

        [XmlIgnore]
        private ICommand refreshStatisticsCommand;
        [XmlIgnore]
        public ICommand RefreshStatisticsCommand
        {
            get
            {
                if (refreshStatisticsCommand == null)
                {
                    refreshStatisticsCommand = new RelayCommand<bool>(refreshStatistics);
                }
                return refreshStatisticsCommand;
            }
        }
        
        private void goToViewTasks()
        {
            TasksFilter.rootList = rootList;
            ViewTasks view = new ViewTasks(Categories);
            view.Show();
        }

        [XmlIgnore]
        private ICommand goToViewTasksCommand;
        [XmlIgnore]
        public ICommand GoToViewTasksCommand
        {
            get
            {
                if (goToViewTasksCommand == null)
                {
                    goToViewTasksCommand = new RelayCommand<bool>(goToViewTasks);
                }
                return goToViewTasksCommand;
            }
        }

        private void saveDatabase()
        {
            const string filePath = @"..\..\DataBases\";
            XmlSerializer ser = new XmlSerializer(typeof(TreeViewVM));
            FileStream writer;

            if (File.Exists(filePath + databaseName + ".xml"))
            {
                writer = File.OpenWrite(filePath + databaseName + ".xml");
            }
            else
            {
                writer = File.Create(filePath + databaseName + ".xml");
            }

            ser.Serialize(writer, this);
            writer.Dispose();
        }

        [XmlIgnore]
        private ICommand saveCommand;
        [XmlIgnore]
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand<bool>(saveDatabase);
                }
                return saveCommand;
            }
        }
    }
}
