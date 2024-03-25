using Avalonia;
using Avalonia.ReactiveUI;
using ClientForAPI.Configuration;
using ClientForAPI.Models.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ClientForAPI
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            new LoggerInit();
            RootSettings Settings = ConfigurationManager.Instance.RootSettings;

            Log.Debug("Запуск сетевого сервиса.");
            Task.Run(() => Models.Backend.ConnectionService.Instance.Start());

            Log.Debug("Запуск приложения.");
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
    }
}
