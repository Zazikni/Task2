using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaClient.ViewModels;
using AvaloniaClient.Views;
using Serilog;

namespace AvaloniaClient.Models.WindowManager
{
    /// <summary>
    /// Класс для управления окнами.
    /// </summary>
    public static class WindowManager
    {
        #region fields

        private static Window? _reg_window = null;

        #endregion fields

        #region methods

        /// <summary>
        /// Метод для открытия окна регистрации.
        /// </summary>
        public static void ShowRegWindow()
        {
            Log.Information($"Open registration window.");

            if (_reg_window == null)
            {
                _reg_window = new RegWindow();
                _reg_window.DataContext = new RegWindowViewModel();
                _reg_window.Show();
            }
        }

        /// <summary>
        /// Метод для сброса состояния окна регистрации.
        /// </summary>
        public static void DropRegWindow()
        {
            _reg_window = null;
        }

        /// <summary>
        /// Метод для закрытия окна регистрации.
        /// </summary>
        public static void CloseRegWindow()
        {
            Log.Information($"Close registration window.");

            if (_reg_window != null)
            {
                _reg_window.Close(_reg_window.DataContext);
            }
        }

        /// <summary>
        /// Метод для закрытия текущего главного окна и переключения на главное (логически) окно программы.
        /// </summary>
        public static void SwitchToMainWindow()
        {
            Log.Information($"Switch to main window");
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var currentMainWindow = desktopLifetime.MainWindow;

                var newWindow = new MainWindow();
                newWindow.DataContext = new MainWindowViewModel();
                newWindow.Show();

                desktopLifetime.MainWindow = newWindow;
                currentMainWindow.Close();
            }
        }

        public static void SwitchToAuthWindow()
        {
            Log.Information($"Switch to auth window");
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var currentMainWindow = desktopLifetime.MainWindow;

                var newWindow = new AuthWindow();
                newWindow.DataContext = new AuthWindowViewModel();
                newWindow.Show();

                desktopLifetime.MainWindow = newWindow;
                currentMainWindow.Close();
            }
        }

        #endregion methods
    }
}