using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.View.Windows;

namespace TaskBoard.ViewModel
{
    public class TaskBoardViewModel
    {
        ApplicationContext _context = new ApplicationContext();
        public ObservableCollection<Model.Task> BacklogTasks { get; private set; }
        public ObservableCollection<Model.Task> ToDoTasks { get; private set; }
        public ObservableCollection<Model.Task> InProgressTasks { get; private set; }
        public ObservableCollection<Model.Task> DoneTasks { get; private set; }
        RelayCommand? _addTaskCommand;
        RelayCommand? _removeTaskCommand;
        RelayCommand? _editTaskCommand;

        public TaskBoardViewModel()
        {
            _context.Tasks.Load();
            BacklogTasks = new(_context.Tasks.Where(t => t.TaskStateId == 1).ToArray());
            ToDoTasks = new(_context.Tasks.Where(t => t.TaskStateId == 2).ToArray());
            InProgressTasks = new(_context.Tasks.Where(t => t.TaskStateId == 3).ToArray());
            DoneTasks = new(_context.Tasks.Where(t => t.TaskStateId == 4).ToArray());
        }

        public RelayCommand AddTaskCommand
        {
            get
            {
                return _addTaskCommand ??
                    (_addTaskCommand = new RelayCommand((o) =>
                    {
                        //TODO
                    }
                    ));
            }
        }
        public RelayCommand RemoveTaskCommand
        {
            get
            {
                return _removeTaskCommand ??
                    (_removeTaskCommand = new RelayCommand((o) =>
                    {
                        if (o is TaskCard tc)
                            DialogWindow.ShowDialog($"Удалить задание \"{tc.Title}\"?");
                        //TODO
                    }
                    ));
            }
        }
        public RelayCommand EditTaskCommand
        {
            get
            {
                return _editTaskCommand ??
                    (_editTaskCommand = new RelayCommand((o) =>
                    {
                        if (o is Model.Task t)
                        {
                            Model.Task task = _context.Tasks.FirstOrDefault(x => x.Id == t.Id);
                            if (task.TaskStateId != t.TaskStateId)
                            {
                                RemoveTaskFromBoard(task);
                                task.TaskStateId = t.TaskStateId;
                                _context.SaveChanges();
                                AddTaskOnBoard(task);
                            }
                        }
                    }
                    ));
            }
        }

        private void AddTaskOnBoard(Model.Task t)
        {
            switch (t.TaskStateId)
            {
                case 1:
                    BacklogTasks.Add(t);
                    break;
                case 2:
                    ToDoTasks.Add(t);
                    break;
                case 3:
                    InProgressTasks.Add(t);
                    break;
                case 4:
                    DoneTasks.Add(t);
                    break;
            }
        }
        private void RemoveTaskFromBoard(Model.Task t)
        {
            BacklogTasks.Remove(t);
            ToDoTasks.Remove(t);
            InProgressTasks.Remove(t);
            DoneTasks.Remove(t);
        }
    }
}
