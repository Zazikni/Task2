using Avalonia.Controls;
using AvaloniaClient.Models.WindowManager;

namespace AvaloniaClient;

public partial class RegWindow : Window
{
    #region constructors

    public RegWindow()
    {
        this.Closing += OnClosing;
        InitializeComponent();
    }

    #endregion constructors

    #region methods

    private async void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        WindowManager.DropRegWindow();
    }

    #endregion methods
}