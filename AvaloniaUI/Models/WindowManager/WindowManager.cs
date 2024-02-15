using Avalonia.Controls;
using ReactiveUI;
using AvaloniaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaUI.ViewModels;
using AvaloniaUI.Views;

namespace AvaloniaUI.Models.WindowManager
{
    public static class WindowManager 
    {
        private static Window? _reg_window = null;

        public static void ShowRegWindow()
        {
            if(_reg_window == null)
            {
                _reg_window = new RegWindow();
                _reg_window.DataContext = new RegWindowViewModel();
                _reg_window.Show();
            }
     
        }
        public static void DropRegWindow()
        {
            _reg_window = null;
        }
        public static void SwitchToMainWindow()
        {
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                // Получаем текущее активное окно
                var currentMainWindow = desktopLifetime.MainWindow;

                // Создать и показать новое окно
                var newWindow = new MainWindow();
                newWindow.DataContext = new MainWindowViewModel();
                newWindow.Show();

                // Установить новое окно как главное
                desktopLifetime.MainWindow = newWindow;
                // Закрыть текущее главное окно
                currentMainWindow.Close();
            }

        }

       
        public static void CloseWindow(Window window)
        {
            window.Close();
        }
        
    }

}
