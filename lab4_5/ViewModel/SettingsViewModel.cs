using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace lab4_5
{
    class SettingsViewModel : INotifyPropertyChanged
    {

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }
        //private string currentLanguage = "ru";
        private int _currentThemeIndex = 0;

        public ICommand ChangeThemeCommand { get; }
        //public ICommand ChangeLanguageCommand { get; }
        public ICommand OpenEditProfileCommand { get; }
        public ICommand OpenOrdersWindowCommand { get; }

        public SettingsViewModel(User user)
        {
            try
            {
                ChangeThemeCommand = new RelayCommand(ChangeTheme);
                //ChangeLanguageCommand = new RelayCommand(ChangeLanguage);
                OpenEditProfileCommand = new RelayCommand(OpenEditProfile);
                OpenOrdersWindowCommand = new RelayCommand(OpenOrdersWindow);
                CurrentUser = user;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        private void OpenEditProfile()
        {
            try
            {
                var editProfileWindow = new EditProfileWindow(CurrentUser);
                editProfileWindow.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private readonly string[] _themes =
        {
            "D:\\лабораторные работы\\ооп\\lab4_5\\lab4_5\\Themes\\DarkTheme.xaml",     // Темная тема
            //"D:\\лабораторные работы\\ооп\\lab4_5\\lab4_5\\Themes\\BrownTheme.xaml",   // Дополнительная тема
            "D:\\лабораторные работы\\ооп\\lab4_5\\lab4_5\\Themes\\DefaultTheme.xaml"
        };
        private void ChangeTheme()
        {
            try
            {
                string themePath = _themes[_currentThemeIndex];

                var themeDict = new ResourceDictionary()
                {
                    Source = new Uri(themePath, UriKind.Absolute)
                };

                var currentheme = Application.Current.Resources.MergedDictionaries
                                  .FirstOrDefault(d => d.Source != null && d.Source.ToString().Contains("Themes"));

                if (currentheme != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(currentheme);
                }

                Application.Current.Resources.MergedDictionaries.Add(themeDict);

                _currentThemeIndex = (_currentThemeIndex + 1) % _themes.Length;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        public void OpenOrdersWindow()
        {
            UserOrdersWindow uow = new UserOrdersWindow(CurrentUser.Id);
            uow.ShowDialog();
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
}
