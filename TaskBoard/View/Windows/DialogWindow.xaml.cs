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
    /// Логика взаимодействия для DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public string Text { get; set; }
        public DialogWindow(string message)
        {
            InitializeComponent();
            Text = message;
            this.DataContext = this;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public static bool ShowDialog(string message)
        {
            
            DialogWindow window = new DialogWindow(message);
            MainWindow.ApplyBlur(10);
            bool? result = window.ShowDialog();
            MainWindow.ApplyBlur(0);

            if (result == true)
                return true;
            return false;
        }
    }
}
