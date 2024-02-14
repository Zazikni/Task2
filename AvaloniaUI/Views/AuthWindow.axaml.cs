using Avalonia.Controls;
using Avalonia.Interactivity;
using Serilog;

namespace AvaloniaUI.Views
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent(attachDevTools: true);
        }
        public void button_ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            
            // Change button text when button is clicked.
            //var button = (Button)sender;
            //Getting Controls references
            //var textBoxLogin = this.FindControl<TextBox>("TextBoxLogin");
            //var textBoxPass = this.FindControl<TextBox>("TextBoxPassword");
            //Log.Debug($"Button {button.Name} was clicked!");
            //Log.Debug($"TextBoxLogin: {textBoxLogin.Text} TextBoxPassword:{textBoxPass.Text}");
        }
    }
}