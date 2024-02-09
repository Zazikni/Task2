using Avalonia.Controls;
using Avalonia.Interactivity;
using UI.Core;

namespace UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(attachDevTools:true);
        }
        public void button_ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            // Change button text when button is clicked.
            var button = (Button)sender;
            button.Content = "Hello, Avaloniaasdasda!";
        }
    }
}
 
 
 
