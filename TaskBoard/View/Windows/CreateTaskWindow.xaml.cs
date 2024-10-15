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
    /// Логика взаимодействия для CreateTaskWindow.xaml
    /// </summary>
    public partial class CreateTaskWindow : Window
    {
        public static Model.Task Task { get; private set; }
        public CreateTaskWindow()
        {
            InitializeComponent();
            Task = new Model.Task();
            DataContext = Task;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public new bool? ShowDialog()
        {
            MainWindow.ApplyBlur(10);
            bool? result = base.ShowDialog();
            MainWindow.ApplyBlur(0);
            return result;
        }
    }
}
