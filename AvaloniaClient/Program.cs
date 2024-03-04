using Avalonia;
using Avalonia.ReactiveUI;
using AvaloniaClient.Models.Backend;
using AvaloniaClient.Models.Logging;
using System;
using Serilog;
using System.Threading.Tasks;

namespace AvaloniaClient
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

            Log.Debug("Запуск сетевого сервиса.");
            Task.Run(() => ConnectionService.Instance.Start());
            
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
