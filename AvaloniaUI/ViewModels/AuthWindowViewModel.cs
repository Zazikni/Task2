using AvaloniaUI.Models.Database;
using Microsoft.Data.Sqlite;
namespace AvaloniaUI.ViewModels
{
    public class AuthWindowViewModel : ViewModelBase
    {
        DatabaseSqlite database = new DatabaseSqlite();

#pragma warning disable CA1822 // Mark members as static
        public string Greeting => "HELLO!";

#pragma warning restore CA1822 // Mark members as static
    }
}
