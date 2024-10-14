using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskBoard.Model;
using TaskBoard.View.Windows;

namespace TaskBoard.View.Components
{
    /// <summary>
    /// Логика взаимодействия для TasksStack.xaml
    /// </summary>
    public partial class TasksStack : UserControl
    {
        public int Id {  get; set; }
        public static readonly DependencyProperty TasksProperty =
            DependencyProperty.Register(
                "Tasks",
                typeof(ObservableCollection<Model.Task>),
                typeof(TasksStack));
        public string Title { get; set; }
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(
                "TextColor",
                typeof(SolidColorBrush),
                typeof(TasksStack));
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(
                "BackgroundColor",
                typeof(SolidColorBrush),
                typeof(TasksStack));
        public ObservableCollection<Model.Task> Tasks
        {
            get { return (ObservableCollection<Model.Task>)GetValue(TasksProperty); }
            set { SetValue(TasksProperty, value); }
        }
        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public ICommand UpdateItem
        {
            get { return (ICommand)GetValue(UpdateItemProperty); }
            set { SetValue(UpdateItemProperty, value); }
        }
        public static readonly DependencyProperty UpdateItemProperty =
            DependencyProperty.Register(
                "UpdateItem",
                typeof(ICommand),
                typeof(TasksStack));

        public TasksStack()
        {
            InitializeComponent();
        }

        private void StackPanel_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object"))
            {
                if (e.KeyStates == DragDropKeyStates.ControlKey)
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.Move;
                }
            }
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Handled == false)
            {
                TaskCard _element = (TaskCard)e.Data.GetData("Object");
                UpdateItem.Execute(new Model.Task() { Id = _element.Id, TaskStateId = Id});
            }
        }

        T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            if (parentObject is T parent)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }
    }
}
