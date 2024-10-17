using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Model;
using TaskBoard.View.Windows;

namespace TaskBoard.ViewModel
{
    public class TaskBoardViewModel : INotifyPropertyChanged
    {
        ApplicationContext _context = new ApplicationContext();
        Board _currentBoard;
        public Board CurrentBoard
        {
            get => _currentBoard;
            set
            {
                _currentBoard = value;
                OnPropertyChanged("CurrentBoard");
            }
        }
        public int CurrentBoardId
        {
            get => _currentBoard.Id;
        }
        public string CurrentBoardTitle
        {
            get => _currentBoard.Title;
        }
        ObservableCollection<Board> _boards;
        public ObservableCollection<Board> Boards
        {
            get => _boards;
            set
            {
                _boards = value;
                OnPropertyChanged("Boards");
            }
        }
        public ObservableCollection<Model.Task> BacklogTasks { get; private set; } = new();
        public ObservableCollection<Model.Task> ToDoTasks { get; private set; } = new();
        public ObservableCollection<Model.Task> InProgressTasks { get; private set; } = new();
        public ObservableCollection<Model.Task> DoneTasks { get; private set; } = new();
        RelayCommand? _addTaskCommand;
        RelayCommand? _removeTaskCommand;
        RelayCommand? _editTaskCommand;
        RelayCommand? _createBoardCommand;
        RelayCommand? _removeBoardCommand;
        RelayCommand? _selectBoardCommand;

        public TaskBoardViewModel()
        {
            _context.Boards.Load();
            _context.Tasks.Load();
            Boards = _context.Boards.Local.ToObservableCollection();
            if (Boards.Count() > 0)
                SetCurrentBoard(_context.Boards.First().Id);
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
                                task.BoardId = CurrentBoardId;
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
                            Boards.Add(window.board);
                            _context.SaveChanges();
                            SetCurrentBoard(Boards.Last().Id);
                        }
                    }
                    ));
            }
        }
        public  RelayCommand SelectBoardCommand
        {
            get
            {
                return _selectBoardCommand ??
                    (_selectBoardCommand = new RelayCommand((o) =>
                    {
                        if (o is int selectedBoardId && selectedBoardId != CurrentBoardId)
                            SetCurrentBoard(selectedBoardId);
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
                            if (board is not null)
                            {
                                if (DialogWindow.ShowDialog($"Удалить \"{board.Title}\"?") == true)
                                {
                                    int currentBoardIndex = Boards.IndexOf(board);
                                    Boards.Remove(board);
                                    BacklogTasks.Clear();
                                    ToDoTasks.Clear();
                                    InProgressTasks.Clear();
                                    DoneTasks.Clear();
                                    if (CurrentBoardId == boardId && Boards.Count > 0)
                                        SetCurrentBoard(Boards[currentBoardIndex - 1].Id);
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                    ));
            }
        }

        void AddTaskOnBoard(Model.Task t)
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
        void RemoveTaskFromBoard(Model.Task t)
        {
            BacklogTasks.Remove(t);
            ToDoTasks.Remove(t);
            InProgressTasks.Remove(t);
            DoneTasks.Remove(t);
        }
        void SetCurrentBoard(int boardId)
        {
            Board? board = _context.Boards.First(a => a.Id == boardId);
            if(board is not null)
            {
                CurrentBoard = board;

                BacklogTasks.Clear();
                ToDoTasks.Clear();
                InProgressTasks.Clear();
                DoneTasks.Clear();

                foreach (Model.Task t in board.Tasks)
                    AddTaskOnBoard(t);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}