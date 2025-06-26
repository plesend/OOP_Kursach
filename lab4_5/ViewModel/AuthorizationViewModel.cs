using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;

namespace lab4_5
{
    public class AuthorizationViewModel : INotifyPropertyChanged
    {
        private string _login;
        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(nameof(Login)); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenRegistrationCommand { get; }

        public AuthorizationViewModel()
        {
            LoginCommand = new RelayCommand(LogIn);
            OpenRegistrationCommand = new RelayCommand(OpenRegistration);
        }

        private void LogIn(object obj)
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            string connectionString = @"Data Source=WIN-0RRORC9T71J\SQLEXPRESS;Initial Catalog=CosmeticShop;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT * FROM Users 
                    WHERE Login COLLATE Latin1_General_BIN = @login 
                      AND Password COLLATE Latin1_General_BIN = @password";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@login", Login);
                    command.Parameters.AddWithValue("@password", Password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var user = new User(
                                id: reader.GetInt32(reader.GetOrdinal("Id")),
                                role: reader.GetString(reader.GetOrdinal("Role")),
                                login: reader.GetString(reader.GetOrdinal("Login")),
                                username: reader.GetString(reader.GetOrdinal("Username")),
                                password: reader.GetString(reader.GetOrdinal("Password")),
                                pfp: reader.GetString(reader.GetOrdinal("Pfp"))
                            );

                            MainWindow main = new MainWindow(user);
                            main.Show();

                            (obj as Window)?.Close();
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль.");
                        }
                    }
                }
            }
        }

        private void OpenRegistration(object obj)
        {
            RegistrationWindow rw = new RegistrationWindow();
            rw.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
