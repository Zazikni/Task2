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

        // Подписываемся на событие изменения текста
        TextBoxPassword.PropertyChanged += PasswordBox_PropertyChanged;
        TextBoxConfirmPassword.PropertyChanged += ConfirmPasswordBox_PropertyChanged;

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
        if (!string.IsNullOrEmpty(TextBoxPassword.Text) && TextBoxPassword.Text == TextBoxConfirmPassword.Text)
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