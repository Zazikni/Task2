using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ClientForAPI.Views
{
    public partial class AuthWindow : Window
    {


        #region Constructors

        public AuthWindow()
        {
            InitializeComponent(attachDevTools: true);

            // Подписываемся на событие изменения текста
            TextBoxPasswordReg.PropertyChanged += PasswordBoxReg_PropertyChanged;
            TextBoxConfirmPasswordReg.PropertyChanged += ConfirmPasswordBoxReg_PropertyChanged;
            TextBoxLoginReg.PropertyChanged += TextBoxLoginReg_PropertyChanged;

            this.Closing += OnClosing;
        }
        #endregion Constructors

        #region Methods

        private async void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //WindowManager.DropRegWindow();
        }
        private void PasswordBoxReg_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            // Проверяем, что изменяемое свойство - это текст
            if (e.Property == TextBox.TextProperty)
            {
                ValidatePasswords();
            }
        }

        private void ConfirmPasswordBoxReg_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            // Проверяем, что изменяемое свойство - это текст
            if (e.Property == TextBox.TextProperty)
            {
                ValidatePasswords();
            }
        }
        private void TextBoxLoginReg_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            // Проверяем, что изменяемое свойство - это текст
            if (e.Property == TextBox.TextProperty)
            {
                ValidateLogin();
            }
        }
        private void ValidatePasswords()
        {
            if (!string.IsNullOrEmpty(TextBoxPasswordReg.Text) && TextBoxPasswordReg.Text == TextBoxConfirmPasswordReg.Text)
            {
                ValidationTextBlockReg.IsVisible = false; // Пароли совпадают
            }
            else
            {
                ValidationTextBlockReg.IsVisible = true;
                ValidationTextBlockReg.Text = "Пароли не совпадают!";
            }
        }
        private void ValidateLogin()
        {
            if (!string.IsNullOrEmpty(TextBoxLoginReg.Text) && !string.IsNullOrWhiteSpace(TextBoxLoginReg.Text))
            {
                ValidationTextBlockReg.IsVisible = false;
            }
            else
            {
                ValidationTextBlockReg.IsVisible = true;
                ValidationTextBlockReg.Text = "Логин не может быть пустым!";
            }
        }


        #endregion Methods
    }
}