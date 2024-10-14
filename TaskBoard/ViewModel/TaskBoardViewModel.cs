using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.View.Windows;

namespace TaskBoard.ViewModel
{
    public class TaskBoardViewModel
    {
        ApplicationContext _context = new ApplicationContext();
        RelayCommand? _addTaskCommand;
        RelayCommand? _removeTaskCommand;
        RelayCommand? _editTaskCommand;

        public TaskBoardViewModel()
        {

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
    }
}
