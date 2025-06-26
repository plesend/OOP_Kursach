using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow(int userId, ObservableCollection<CartItem> cartItems)
        {
            InitializeComponent();
            DataContext = new OrderViewModel(userId, cartItems);
        }
        public void Decline_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
