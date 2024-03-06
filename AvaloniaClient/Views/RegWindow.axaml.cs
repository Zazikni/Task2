using Avalonia;
using Avalonia.Controls;
using AvaloniaClient.Models.WindowManager;

namespace AvaloniaClient;

public partial class RegWindow : Window
{
    #region Fields


    #endregion Fields

    #region Constructors

    public RegWindow()
    {
        InitializeComponent();
        //PasswordBox = this.FindControl<TextBox>("PasswordBox");
        //ConfirmPasswordBox = this.FindControl<TextBox>("ConfirmPasswordBox");
        //ValidationTextBlock = this.FindControl<TextBlock>("ValidationTextBlock");
        // Подписываемся на событие изменения текста
        PasswordBox.PropertyChanged += PasswordBox_PropertyChanged;
        ConfirmPasswordBox.PropertyChanged += ConfirmPasswordBox_PropertyChanged;

        this.Closing += OnClosing;
    }

    #endregion Constructors

    #region Methods

    private async void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        WindowManager.DropRegWindow();
    }
    private void PasswordBox_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        // Проверяем, что изменяемое свойство - это текст
        if (e.Property == TextBox.TextProperty)
        {
            ValidatePasswords();
        }
    }

    private void ConfirmPasswordBox_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        // Проверяем, что изменяемое свойство - это текст
        if (e.Property == TextBox.TextProperty)
        {
            ValidatePasswords();
        }
    }
    private void ValidatePasswords()
    {
        if (!string.IsNullOrEmpty(PasswordBox.Text) && PasswordBox.Text == ConfirmPasswordBox.Text)
        {
            ValidationTextBlock.IsVisible = false; // Пароли совпадают
        }
        else
        {
            ValidationTextBlock.IsVisible = true;
            ValidationTextBlock.Text = "Пароли не совпадают!";
        }
    }


    #endregion Methods
}