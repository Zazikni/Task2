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

        private Window? _auth_window = null;
        public Window GetAuthWindow { get { return _auth_window; } }
        private Window? _main_window = null;
        public Window GetMainWindow { get { return _main_window; } }



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
                var current_main_window = desktopLifetime.MainWindow;

                var new_main_window = new MainWindow();
                new_main_window.DataContext = new MainWindowViewModel();
                _main_window = new_main_window;
                desktopLifetime.MainWindow = new_main_window;
                new_main_window.Show();


                current_main_window.Close();
            }
        }

        public void SwitchToAuthWindow()
        {
            Log.Information($"Переключение на окно аутентификации.");
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var current_main_window = desktopLifetime.MainWindow;

                var new_auth_window = new AuthWindow();
                new_auth_window.DataContext = new AuthWindowViewModel();
                new_auth_window.Show();

                _auth_window = new_auth_window;
                desktopLifetime.MainWindow = new_auth_window;
                current_main_window.Close();
            }
        }

        #endregion Methods
    }
}