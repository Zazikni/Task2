using Avalonia;
using Avalonia.Controls;
using ClientForAPI.Models.WindowManager;

namespace ClientForAPI;

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
        TextBoxLogin.PropertyChanged += TextBoxLogin_PropertyChanged;

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
    private void TextBoxLogin_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        // Проверяем, что изменяемое свойство - это текст
        if (e.Property == TextBox.TextProperty)
        {
            ValidateLogin();
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
    private void ValidateLogin()
    {
        if (!string.IsNullOrEmpty(TextBoxLogin.Text) && !string.IsNullOrWhiteSpace(TextBoxLogin.Text))
        {
            ValidationTextBlock.IsVisible = false; 
        }
        else
        {
            ValidationTextBlock.IsVisible = true;
            ValidationTextBlock.Text = "Логин не может быть пустым!";
        }
    }


    #endregion Methods
}