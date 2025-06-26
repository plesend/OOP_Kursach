using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace lab4_5
{
    public class EditProfileViewModel : INotifyPropertyChanged
    {
        public Action CloseAction { get; set; }
        public event Action<User> UserChanged;
        private User _currentUser;
        private string _newUserName;
        private string _newPassword;
        private string _errorMessage;
        private string _profilePicBuffer;

        public event PropertyChangedEventHandler PropertyChanged;

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public string Pfp
        {
            get => _profilePicBuffer;
            set
            {
                _profilePicBuffer = value;
                OnPropertyChanged();
            }
        }

        public string newUserName
        {
            get => _newUserName;
            set
            {
                _newUserName = value;
                OnPropertyChanged();
            }
        }

        public string newPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                OnPropertyChanged();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }


        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ChangeProfileImageCommand { get; }

        public EditProfileViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            newUserName = currentUser.Username;
            _profilePicBuffer = currentUser.Pfp;

            SaveCommand = new RelayCommand(Save);
            CloseCommand = new RelayCommand(Close);
            ChangeProfileImageCommand = new RelayCommand(ChangeProfileImage);
        }

        private string connectionString = "Data Source=WIN-0RRORC9T71J\\SQLEXPRESS;Initial Catalog=CosmeticShop;TrustServerCertificate=Yes;Integrated Security=True;";

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(newUserName))
            {
                MessageBox.Show("Имя пользователя не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newUserName.Length > 25)
            {
                MessageBox.Show("Имя пользователя не должно превышать 25 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isPasswordChanged = !string.IsNullOrWhiteSpace(newPassword) && newPassword != CurrentUser.Password;

            if (isPasswordChanged)
            {
                if (string.IsNullOrWhiteSpace(OldPassword))
                {
                    MessageBox.Show("Для изменения пароля необходимо указать текущий пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (newPassword != ConfirmPassword)
                {
                    MessageBox.Show("Новый пароль и подтверждение не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string checkPassQuery = "SELECT Password FROM Users WHERE Id = @Id";
                        SqlCommand checkCmd = new SqlCommand(checkPassQuery, conn);
                        checkCmd.Parameters.AddWithValue("@Id", CurrentUser.Id);
                        var dbPasswordObj = checkCmd.ExecuteScalar();
                        string dbPassword = dbPasswordObj != null ? dbPasswordObj.ToString() : null;

                        if (dbPassword == null || dbPassword != OldPassword)
                        {
                            MessageBox.Show("Неверный текущий пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        string updateQuery = @"
                    UPDATE Users
                    SET Username = @Username,
                        Pfp = @Pfp,
                        Password = @Password
                    WHERE Id = @Id";

                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@Username", newUserName);
                        updateCmd.Parameters.AddWithValue("@Pfp", Pfp);
                        updateCmd.Parameters.AddWithValue("@Password", newPassword);
                        updateCmd.Parameters.AddWithValue("@Id", CurrentUser.Id);

                        updateCmd.ExecuteNonQuery();

                        CurrentUser.Username = newUserName;
                        CurrentUser.Pfp = Pfp;
                        CurrentUser.Password = newPassword;

                        MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        UserChanged?.Invoke(CurrentUser);
                        CloseAction?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string updateQuery = @"
                    UPDATE Users
                    SET Username = @Username,
                        Pfp = @Pfp
                    WHERE Id = @Id";

                        SqlCommand cmd = new SqlCommand(updateQuery, conn);
                        cmd.Parameters.AddWithValue("@Username", newUserName);
                        cmd.Parameters.AddWithValue("@Pfp", Pfp);
                        cmd.Parameters.AddWithValue("@Id", CurrentUser.Id);

                        cmd.ExecuteNonQuery();

                        CurrentUser.Username = newUserName;
                        CurrentUser.Pfp = Pfp;

                        MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        UserChanged?.Invoke(CurrentUser);
                        CloseAction?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Close()
        {
            CurrentUser.Username = newUserName;
            CurrentUser.Pfp = Pfp;

            CloseAction?.Invoke();
        }

        private void ChangeProfileImage()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                Pfp = dialog.FileName;
            }
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
