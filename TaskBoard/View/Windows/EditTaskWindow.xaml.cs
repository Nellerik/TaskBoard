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

namespace TaskBoard.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        readonly Model.Task Task;
        public EditTaskWindow(Model.Task task)
        {
            InitializeComponent();
            Task = task;
            DataContext = new Model.Task() { Title = task.Title, Description = task.Description };
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Task.Title = (DataContext as Model.Task).Title;
            Task.Description = (DataContext as Model.Task).Description;
        }
        public bool? ShowDialog()
        {
            MainWindow.ApplyBlur(10);
            bool? result = base.ShowDialog();
            MainWindow.ApplyBlur(0);
            return result;
        }
    }
}
