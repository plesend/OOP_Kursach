using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace lab4_5
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Обработчик исключений в UI потоке (главный поток)
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Обработчик необработанных исключений в других потоках
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        // Обработка ошибок в UI потоке
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Логируем ошибку
            LogError(e.Exception);

            // Отображаем сообщение пользователю
            MessageBox.Show("Произошла ошибка приложения. Мы уже работаем над её исправлением.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            // Предотвращаем завершение приложения
            e.Handled = true;
        }

        // Обработка ошибок в других потоках
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                // Логируем ошибку
                LogError(exception);
            }

            // Можно добавить логику для завершения приложения в случае фатальной ошибки
            // Например:
            // Environment.Exit(1); // Завершаем приложение с кодом ошибки
        }

        // Логирование ошибки в файл
        private void LogError(Exception ex)
        {
            string logFilePath = "D:\\лабораторные работы\\ооп\\lab4_5\\app_error_log.txt";
            string logMessage = $"Date: {DateTime.Now}\nMessage: {ex.Message}\nStack Trace: {ex.StackTrace}\n{new string('-', 50)}\n";

            // Записываем ошибку в файл
            File.AppendAllText(logFilePath, logMessage);
        }
    }

}
