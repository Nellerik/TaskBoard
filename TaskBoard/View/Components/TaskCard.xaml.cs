using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskBoard.View.Windows;

namespace TaskBoard
{
    /// <summary>
    /// Логика взаимодействия для TaskCard.xaml
    /// </summary>
    public partial class TaskCard : UserControl
    {
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(TaskCard));
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TaskCard));
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(TaskCard));
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly RoutedEvent EditCardClickedEvent = 
            EventManager.RegisterRoutedEvent("EditCardClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskCard));
        public static readonly RoutedEvent RemoveCardClickedEvent =
            EventManager.RegisterRoutedEvent("RemoveCardClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskCard));
        public event RoutedEventHandler EditCardClicked
        {
            add { AddHandler(EditCardClickedEvent, value); }
            remove { RemoveHandler(EditCardClickedEvent, value); }
        }
        public event RoutedEventHandler RemoveCardClicked
        {
            add { AddHandler(RemoveCardClickedEvent, value); }
            remove { RemoveHandler(RemoveCardClickedEvent, value); }
        }
        public TaskCard()
        {
            InitializeComponent();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("Object", this);
                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(EditCardClickedEvent);
            RaiseEvent(newEventArgs);

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow.ShowDialog($"Вы действительно хотите удалить задачу \"{Title}\"?");
            RoutedEventArgs newEventArgs = new RoutedEventArgs(RemoveCardClickedEvent);
            RaiseEvent(newEventArgs);
        }
    }
}
