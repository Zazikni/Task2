using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaUI.ViewModels;
using AvaloniaUI.Views;
using Serilog;
using Serilog.Events;
using System;
using System.Configuration;

namespace AvaloniaUI
{
    public partial class App : Application
    {
        int logFileSize = Convert.ToInt32(ConfigurationManager.AppSettings["logMaxSizeInBytes"]);
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

        }

        public override void OnFrameworkInitializationCompleted()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(path: "logs/debug-.log", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: logFileSize)
                .WriteTo.File(path: "logs/info-.log", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: logFileSize)
                .WriteTo.File(path: "logs/warn-.log", restrictedToMinimumLevel: LogEventLevel.Warning, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: logFileSize)
                .WriteTo.File(path: "logs/error-.log", restrictedToMinimumLevel: LogEventLevel.Error, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: logFileSize)
                .WriteTo.Console()
                .CreateLogger();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new AuthWindow
                {
                    DataContext = new AuthWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}