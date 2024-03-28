using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ClientForAPI.Models.RemoteServices;
using ClientForAPI.ViewModels;
using ClientForAPI.Views;
using Serilog;

namespace ClientForAPI.Models.LocalServices
{
    /// <summary>
    /// Класс для управления окнами.
    /// </summary>
    public class WindowManagerService
    {
        #region Fields

        private Window? _reg_window = null;
        private Window _current_main_window;
        public Window GetMainWindow { get { return _current_main_window; } }

        private static WindowManagerService? _instance = null;

        public static WindowManagerService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WindowManagerService();
                }
                return _instance;
            }
        }
        #endregion Fields

        #region Contructors
        private WindowManagerService()
        {
            Log.Information($"Инициализации сервиса управления окнами.");
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                _current_main_window = desktopLifetime.MainWindow;
            }

        }

        #endregion Contructors

        #region Methods

        /// <summary>
        /// Метод для закрытия текущего главного окна и переключения на главное (логически) окно программы.
        /// </summary>
        public void SwitchToMainWindow()
        {
            Log.Information($"Переключение на главное окно.");
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var currentMainWindow = desktopLifetime.MainWindow;

                var newWindow = new MainWindow();
                newWindow.DataContext = new MainWindowViewModel();
                newWindow.Show();

                desktopLifetime.MainWindow = newWindow;
                currentMainWindow.Close();
            }
        }

        public void SwitchToAuthWindow()
        {
            Log.Information($"Переключение на окно аутентификации.");
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var currentMainWindow = desktopLifetime.MainWindow;

                var newWindow = new AuthWindow();
                newWindow.DataContext = new AuthWindowViewModel();
                newWindow.Show();

                desktopLifetime.MainWindow = newWindow;
                currentMainWindow.Close();
            }
        }

        #endregion Methods
    }
}