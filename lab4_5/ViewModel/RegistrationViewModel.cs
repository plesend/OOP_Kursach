using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Data.SqlClient;
using System;

namespace lab4_5
{
    public class RegistrationViewModel : INotifyPropertyChanged
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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); }
        }

        public ICommand RegisterCommand { get; }

        public RegistrationViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
        }

        private void Register(object obj)
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                MessageBox.Show("Логин не может быть пустым.");
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Пароль не может быть пустым.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                MessageBox.Show("Подтвердите пароль.");
                return;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            if (Login.Length < 4)
            {
                MessageBox.Show("Логин должен содержать минимум 4 символа.");
                return;
            }
            if (Password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов.");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(Login, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Логин должен содержать только латинские буквы и цифры.");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(Password, @"^(?=.*[a-zA-Z])(?=.*\d).+$"))
            {
                MessageBox.Show("Пароль должен содержать минимум одну букву и одну цифру.");
                return;
            }

            string connectionString = @"Data Source=WIN-0RRORC9T71J\SQLEXPRESS;Initial Catalog=CosmeticShop;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var checkCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Login = @Login", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Login", Login);
                    int userCount = (int)checkCommand.ExecuteScalar();
                    if (userCount > 0)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует.");
                        return;
                    }
                }

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var command = new SqlCommand("INSERT INTO Users (Login, Password) VALUES (@Login, @Password);", connection, transaction);
                        command.Parameters.AddWithValue("@Login", Login);
                        command.Parameters.AddWithValue("@Password", Password);

                        command.ExecuteNonQuery();
                        transaction.Commit();

                        MessageBox.Show("Регистрация прошла успешно!");
                        (obj as Window)?.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
                    }
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}