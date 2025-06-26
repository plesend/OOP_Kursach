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

namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class EditProfileWindow : Window
    {
        public EditProfileWindow(User user)
        {
            InitializeComponent();
            var model = new EditProfileViewModel(user)
            {
                CloseAction = Close
            };
            DataContext = model;
        }
        private void PasswordBox_PassChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((EditProfileViewModel)this.DataContext).newPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}
