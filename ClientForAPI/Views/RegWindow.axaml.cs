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

        // ������������� �� ������� ��������� ������
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
        // ���������, ��� ���������� �������� - ��� �����
        if (e.Property == TextBox.TextProperty)
        {
            ValidatePasswords();
        }
    }

    private void ConfirmPasswordBox_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        // ���������, ��� ���������� �������� - ��� �����
        if (e.Property == TextBox.TextProperty)
        {
            ValidatePasswords();
        }
    }
    private void TextBoxLogin_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        // ���������, ��� ���������� �������� - ��� �����
        if (e.Property == TextBox.TextProperty)
        {
            ValidateLogin();
        }
    }
    private void ValidatePasswords()
    {
        if (!string.IsNullOrEmpty(TextBoxPassword.Text) && TextBoxPassword.Text == TextBoxConfirmPassword.Text)
        {
            ValidationTextBlock.IsVisible = false; // ������ ���������
        }
        else
        {
            ValidationTextBlock.IsVisible = true;
            ValidationTextBlock.Text = "������ �� ���������!";
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
            ValidationTextBlock.Text = "����� �� ����� ���� ������!";
        }
    }


    #endregion Methods
}