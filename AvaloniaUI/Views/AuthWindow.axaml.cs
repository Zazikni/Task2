using Avalonia.Controls;
using Avalonia.Interactivity;

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
            var button = (Button)sender;
            button.Content = "Hello, Avaloniaasdasda!";
        }
    }
}