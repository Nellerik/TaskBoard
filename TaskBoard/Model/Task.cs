using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskBoard.Model
{
    public class Task : INotifyPropertyChanged
    {
        string _title;
        string _description;
        int _taskStateId;
        public int Id { get; set; }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }
        public int TaskStateId
        {
            get => _taskStateId;
            set
            {
                _taskStateId = value;
                OnPropertyChanged("TaskStateId");
            }
        }
        public TaskState TaskState { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}