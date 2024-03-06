using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaClient.ViewModels;
using AvaloniaClient.Views;

namespace AvaloniaClient
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
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