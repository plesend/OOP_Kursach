using System.Windows;
using System.Windows.Controls;

namespace lab4_5
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            DataContext = new AuthorizationViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AuthorizationViewModel viewModel)
            {
                viewModel.Password = (sender as PasswordBox)?.Password ?? "";
            }
        }
    }
}
