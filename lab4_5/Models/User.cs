using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_5
{
    public class User : INotifyPropertyChanged
    {
        private int _id;
        private string _role = "Client";
        private string _login;
        private string _username = "User";
        private string _password;
        private string _pfp = @"D:\лабораторные работы\ооп\lab4_5\lab4_5\Resources\DefaultPfp.png";

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Role
        {
            get => _role;
            set { _role = value; OnPropertyChanged(nameof(Role)); }
        }

        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(nameof(Login)); }
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string Pfp
        {
            get => _pfp;
            set { _pfp = value; OnPropertyChanged(nameof(Pfp)); }
        }

        public User(string role, string login, string username, string password, string pfp)
        {
            Role = role;
            Login = login;
            Username = username;
            Password = password;
            Pfp = pfp;
        }

        public User(int id, string role, string login, string username, string password, string pfp)
        {
            Id = id;
            Role = role;
            Login= login;
            Username = username;
            Pfp = pfp;
        }

        public User() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
