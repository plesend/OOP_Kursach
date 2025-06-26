using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace lab4_5
{
    public class AdminEditUserViewModel : INotifyPropertyChanged
    {
        private User _user;
        public User User
        {
            get => _user;
            set { _user = value; OnPropertyChanged(); }
        }

        public ICommand BrowseImageCommand { get; }
        public ICommand SaveUserCommand { get; }

        public AdminEditUserViewModel(User user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));

            BrowseImageCommand = new RelayCommand(_ => BrowseImage());
            SaveUserCommand = new RelayCommand(_ => SaveUser());
        }

        private void BrowseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Все файлы (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                User.Pfp = openFileDialog.FileName;
                OnPropertyChanged(nameof(User)); 
            }
        }

        private void SaveUser()
        {
            if (string.IsNullOrWhiteSpace(User.Login))
            {
                MessageBox.Show("Логин не может быть пустым.");
                return;
            }

            Window window = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this);

            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
