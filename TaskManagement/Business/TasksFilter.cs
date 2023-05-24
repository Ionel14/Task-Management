using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskOrganizer.Model;
using Task = TaskOrganizer.Model.Task;

namespace TaskOrganizer.Business
{
    internal class TasksFilter
    {
        public static ObservableCollection<ToDoList> rootList;
        private List<Task> allTasks = new List<Task>();

        public ObservableCollection<TaskInfo> Find(string name, DateTime deadline)
        {
            int indexCondition;
            if (name != string.Empty && deadline != DateTime.MinValue)
            {
                indexCondition = 0;
            }
            else if (name != string.Empty)
            {
                indexCondition = 1;
            }
            else
            {
                indexCondition = 2;
            }

            Dictionary<string, string> parent = new Dictionary<string, string>();
            Stack<ToDoList> toDoLists = new Stack<ToDoList>();

            foreach (ToDoList tdl in rootList)
            {
                toDoLists.Push(tdl);
                parent.Add(tdl.Name, String.Empty);
            }

            ToDoList currentTdl;
            ObservableCollection<TaskInfo> tasksFound = new ObservableCollection<TaskInfo>();
            bool conditionValue = false;
            while (toDoLists.Count > 0)
            {
                currentTdl = toDoLists.Pop();

                foreach (Task task in currentTdl.Tasks)
                {
                    switch (indexCondition)
                    {
                        case 0:
                            {
                                if (task.Name.StartsWith(name) && task.Deadline.Date == deadline.Date)
                                {
                                    conditionValue = true;
                                }
                                break;
                            }
                        case 1:
                            {
                                if (task.Name.StartsWith(name))
                                {
                                    conditionValue = true;
                                }
                                break;
                            }
                        case 2:
                            {
                                if (task.Deadline.Date == deadline.Date)
                                {
                                    conditionValue = true;
                                }
                                break;
                            }
                    }

                    if (conditionValue)
                    {
                        string tdlName = currentTdl.Name;
                        string location = tdlName;
                        while (parent[tdlName] != string.Empty)
                        {
                            tdlName = parent[tdlName];
                            location = tdlName + " >> " + location;
                        }

                        TaskInfo taskInfo = new TaskInfo(task.Name, location, currentTdl.ImagePath);
                        tasksFound.Add(taskInfo);
                        conditionValue = false;
                    }
                }

                foreach (ToDoList tdl in currentTdl.subList)
                {
                    toDoLists.Push(tdl);
                    parent.Add(tdl.Name, currentTdl.Name);
                }

            }

            return tasksFound;
        }

        private void getTasks()
        {
            allTasks.Clear();
            Stack<ToDoList> toDoLists = new Stack<ToDoList>();

            foreach (ToDoList tdl in rootList)
            {
                toDoLists.Push(tdl);
            }

            ToDoList currentTdl;
            while (toDoLists.Count > 0)
            {
                currentTdl = toDoLists.Pop();

                foreach (Task task in currentTdl.Tasks)
                {
                    allTasks.Add(task);
                }

                foreach (ToDoList tdl in currentTdl.subList)
                {
                    if (tdl != null)
                    {
                        toDoLists.Push(tdl);
                    }
                }
            }


        }

        public ObservableCollection<Task> getAllTasks()
        {
            filteredByCategory = false;
            getTasks();
            return new ObservableCollection<Task>(allTasks);
        }

        public ObservableCollection<Task> sortByDeadline()
        {
            allTasks.Sort((task1, task2) => task1.Deadline.CompareTo(task2.Deadline));
            return new ObservableCollection<Task>(allTasks);
        }

        public ObservableCollection<Task> sortByPriority()
        {
            allTasks.Sort((task1, task2) =>
            {
                int priority1 = Task.Priorities.IndexOf(task1.Priority);
                int priority2 = Task.Priorities.IndexOf(task2.Priority);

                if (priority1 == priority2)
                {
                    return 0;
                }

                if (priority1 == -1)
                {
                    return 1;
                }

                if (priority2 == -1)
                {
                    return -1;
                }

                return priority1.CompareTo(priority2);
            });
            return new ObservableCollection<Task>(allTasks);
        }

        private bool filteredByCategory = false;
        public ObservableCollection<Task> FilterByCategory(Category category)
        {
            if (filteredByCategory)
            {
                getTasks();
            }
            else
            {
                filteredByCategory = true;
            }

            allTasks = allTasks.Where(task => task.Category.name == category.name).ToList();
            return new ObservableCollection<Task>(allTasks);
        }

        public ObservableCollection<Task> DoneStatusTasks()
        {
            allTasks = allTasks.Where(task => task.Status == Task.Statuses[2]).ToList();
            return new ObservableCollection<Task>(allTasks);
        }
        
        public ObservableCollection<Task> OverdueTasks()
        {
            allTasks = allTasks.Where(task => task.TaskDoneDate != DateTime.MinValue && task.TaskDoneDate.Date >= task.Deadline.Date).ToList();
            return new ObservableCollection<Task>(allTasks);
        }

        public ObservableCollection<Task> ShouldBeFinishedTasks()
        {
            allTasks = allTasks.Where(task => task.TaskDoneDate == DateTime.MinValue &&  task.Deadline <= DateTime.Today).ToList();
            return new ObservableCollection<Task>(allTasks);
        }

        public ObservableCollection<Task> UnfinishedTasks()
        {
            allTasks = allTasks.Where(task => task.TaskDoneDate == DateTime.MinValue && task.Deadline > DateTime.Today).ToList();
            return new ObservableCollection<Task>(allTasks);
        }

    }
}
