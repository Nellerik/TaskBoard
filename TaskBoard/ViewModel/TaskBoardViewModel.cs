using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Model;
using TaskBoard.View.Windows;

namespace TaskBoard.ViewModel
{
    public class TaskBoardViewModel
    {
        ApplicationContext _context = new ApplicationContext();
        int currentBoardId;
        public ObservableCollection<Board> boards {  get; private set; }
        public ObservableCollection<Model.Task> BacklogTasks { get; private set; }
        public ObservableCollection<Model.Task> ToDoTasks { get; private set; }
        public ObservableCollection<Model.Task> InProgressTasks { get; private set; }
        public ObservableCollection<Model.Task> DoneTasks { get; private set; }
        RelayCommand? _addTaskCommand;
        RelayCommand? _removeTaskCommand;
        RelayCommand? _editTaskCommand;
        RelayCommand? _createBoardCommand;
        RelayCommand? _removeBoardCommand;

        public TaskBoardViewModel()
        {
            _context.Boards.Load();
            boards = _context.Boards.Local.ToObservableCollection();
            currentBoardId = boards.First().Id;
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
                        if (o is int stateId)
                        {
                            CreateTaskWindow window = new CreateTaskWindow();
                            if (window.ShowDialog() == true)
                            {
                                Model.Task task = CreateTaskWindow.Task;
                                task.TaskStateId = stateId;
                                task.BoardId = currentBoardId;
                                AddTaskOnBoard(task);
                                _context.Tasks.Add(task);
                                _context.SaveChanges();
                            }
                        }
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
                        if (o is int taskId)
                        {
                            Model.Task task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
                            if (task is not null)
                            {
                                if (DialogWindow.ShowDialog($"Удалить задачу {task.Title}?"))
                                {
                                    RemoveTaskFromBoard(task);
                                    _context.Tasks.Remove(task);
                                    _context.SaveChanges();
                                }
                            }
                        }
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
                        else if (o is int taskId)
                        {
                            Model.Task task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
                            EditTaskWindow window = new EditTaskWindow(task);
                            if (window.ShowDialog() == true)
                            {
                                _context.SaveChanges(true);
                            }
                        }
                    }
                    ));
            }
        }
        public RelayCommand CreateBoardCommand
        {
            get
            {
                return _createBoardCommand ??
                    (_createBoardCommand = new RelayCommand((o) =>
                    {
                        CreateBoardWindow window = new CreateBoardWindow();
                        if(window.ShowDialog() == true)
                        {
                            boards.Add(window.board);
                            _context.SaveChanges();
                        }
                    }
                    ));
            }
        }
        public RelayCommand RemoveBoardCommand
        {
            get
            {
                return _removeBoardCommand ??
                    (_removeBoardCommand = new RelayCommand((o) =>
                    {
                        if (o is int boardId)
                        {
                            Board board = _context.Boards.First(b => b.Id ==  boardId);
                            if (board is not null && boards.Count > 1)
                            {
                                if (DialogWindow.ShowDialog($"Удалить \"{board.Title}\"?") == true)
                                {
                                    boards.Remove(board);
                                    _context.SaveChanges();
                                }
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
