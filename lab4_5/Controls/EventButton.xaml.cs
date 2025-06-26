using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace lab4_5.Controls
{
    public partial class EventButton : UserControl
    {
        public static readonly RoutedUICommand MyCustomCommand = new RoutedUICommand(
            "My Custom Command",    
            "MyCustomCommand",      
            typeof(EventButton)     
        );

        public EventButton()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(MyCustomCommand, ExecuteMyCustomCommand, CanExecuteMyCustomCommand));
        }

        private void ExecuteMyCustomCommand(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("My Custom Command Executed!");
        }

        private void CanExecuteMyCustomCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnPreviewMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            //в самом начале
            MessageBox.Show("PreviewMouseDown (Tunneling) in EventButton");
        }

        private void OnMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            //после PreviewMouseDown
            MessageBox.Show("MouseDown (Bubbling) in EventButton");
        }

        public static readonly RoutedEvent IconClickEvent = EventManager.RegisterRoutedEvent(
            "IconClick", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(EventButton));

        public event RoutedEventHandler IconClick
        {
            add { AddHandler(IconClickEvent, value); }
            remove { RemoveHandler(IconClickEvent, value); }
        }

        //когда отпускает кнопку
        private void OnMouseLeftButtonUpHandler(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(IconClickEvent));
            MessageBox.Show("IconClick (Direct) in EventButton");
        }
    }
}
