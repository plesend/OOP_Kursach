using lab4_5;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows;
using System;

class UserViewModel : INotifyPropertyChanged
{
    public string ConnectionString = "Data source = WIN-0RRORC9T71J\\SQLEXPRESS; Initial Catalog = CosmeticShop;TrustServerCertificate=Yes;Integrated Security=True;TrustServerCertificate=True;";

    private ObservableCollection<User> _users;
    public ObservableCollection<User> Users
    {
        get => _users;
        set { _users = value; OnPropertyChanged(); }
    }

    private User _selectedUser;
    public User SelectedUser
    {
        get => _selectedUser;
        set { _selectedUser = value; OnPropertyChanged(); }
    }

    private readonly string _currentUserLogin;

    public ICommand AssignAdminCommand { get; }
    public ICommand RemoveAdminCommand { get; }
    public ICommand OpenAdminEditUserCommand { get; }

    public UserViewModel(User currentUser)
    {
        _currentUserLogin = currentUser.Login;

        OpenAdminEditUserCommand = new RelayCommand(_ => OpenAdminEditUser(), _ => SelectedUser != null);
        AssignAdminCommand = new RelayCommand(_ => AssignAdmin(), _ => SelectedUser != null);
        RemoveAdminCommand = new RelayCommand(_ => RemoveAdmin(), _ => SelectedUser != null);

        LoadUsers();
    }
    public void OpenAdminEditUser()
    {
        if (SelectedUser == null)
        {
            MessageBox.Show("Пользователь не выбран.");
            return;
        }

        var userCopy = new User
        {
            Username = SelectedUser.Username,
            Login = SelectedUser.Login,
            Password = SelectedUser.Password,
            Pfp = SelectedUser.Pfp,
            Role = SelectedUser.Role
        };

        var editWindow = new AdminEditUserWindow
        {
            DataContext = new AdminEditUserViewModel(userCopy)
        };

        if (editWindow.ShowDialog() == true)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Users SET Username = @Username, Password = @Password, Pfp = @Pfp WHERE Login = @Login",
                    connection);

                command.Parameters.AddWithValue("@Username", userCopy.Username ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Password", userCopy.Password ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Pfp", userCopy.Pfp ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Login", userCopy.Login);

                command.ExecuteNonQuery();
            }

            SelectedUser.Username = userCopy.Username;
            SelectedUser.Password = userCopy.Password;
            SelectedUser.Pfp = userCopy.Pfp;

            OnPropertyChanged(nameof(Users));
        }
    }
    public void LoadUsers()
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Users", connection);
            using (var reader = command.ExecuteReader())
            {
                var users = new ObservableCollection<User>();
                while (reader.Read())
                {
                    users.Add(new User
                    (
                        role: reader["Role"].ToString(),
                        login: reader["Login"].ToString(),
                        username: reader["Username"].ToString(),
                        password: reader["Password"].ToString(),
                        pfp: reader["Pfp"].ToString()
                    ));
                }
                Users = users;
            }
        }
    }

    private void AssignAdmin()
    {
        if (SelectedUser == null)
        {
            MessageBox.Show("Пользователь не выбран.");
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedUser.Login))
        {
            MessageBox.Show("Логин не указан.");
            return;
        }

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var command = new SqlCommand("UPDATE Users SET Role = 'Admin' WHERE Login = @Login", connection);
            command.Parameters.AddWithValue("@Login", SelectedUser.Login);

            int rows = command.ExecuteNonQuery();

            if (rows > 0)
            {
                SelectedUser.Role = "Admin";
                OnPropertyChanged(nameof(Users));
            }
            else
            {
                MessageBox.Show($"Не удалось обновить роль. Логин: {SelectedUser.Login}");
            }
        }
    }

    private void RemoveAdmin()
    {
        if (SelectedUser == null)
        {
            MessageBox.Show("Пользователь не выбран.");
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedUser.Login))
        {
            MessageBox.Show("Логин не указан.");
            return;
        }

        if (SelectedUser.Login == _currentUserLogin)
        {
            MessageBox.Show("Нельзя снять роль администратора с самого себя.");
            return;
        }

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var command = new SqlCommand("UPDATE Users SET Role = 'Client' WHERE Login = @Login", connection);
            command.Parameters.AddWithValue("@Login", SelectedUser.Login);

            int rows = command.ExecuteNonQuery();

            if (rows > 0)
            {
                SelectedUser.Role = "Client";
                OnPropertyChanged(nameof(Users));
            }
            else
            {
                MessageBox.Show($"Не удалось снять роль администратора. Логин: {SelectedUser.Login}");
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        try
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }
}
