using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaUI.Models.WindowManager;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.ComponentModel;

namespace AvaloniaUI;

public partial class RegWindow : Window
{
    #region constructors
    public RegWindow()
    {
        this.Closing += OnClosing;
        InitializeComponent();

    }
    #endregion

    #region methods
    private async void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        WindowManager.DropRegWindow();

    }
    #endregion

}