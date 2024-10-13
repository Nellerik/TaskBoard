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

namespace TaskBoard.View.Components
{
    /// <summary>
    /// Логика взаимодействия для TasksStack.xaml
    /// </summary>
    public partial class TasksStack : UserControl
    {
        public ObservableCollection<Model.Task> Tasks { get; set; } = new ObservableCollection<Model.Task>();

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
        public TasksStack()
        {
            InitializeComponent();
            Tasks.Add(new Model.Task { Title = "Title1", Description = "Description 1" });
            this.DataContext = this;
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
                ItemsControl _panel = (ItemsControl)sender;
                TaskCard _element = (TaskCard)e.Data.GetData("Object");

                if (_panel != null && _element != null)
                {
                    ItemsControl _parent = FindParent<ItemsControl>(_element);
                    ObservableCollection<Model.Task> parentTasks = _parent.ItemsSource as ObservableCollection<Model.Task>;
                    Model.Task _elementModel = parentTasks.FirstOrDefault((e) => e.Title == _element.Title);
                    parentTasks.Remove(_elementModel);
                    (_panel.ItemsSource as ObservableCollection<Model.Task>).Add(_elementModel);
                    e.Effects = DragDropEffects.Move;
                }
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
