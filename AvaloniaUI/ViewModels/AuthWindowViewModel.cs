using AvaloniaUI.Models.Database;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System;
namespace AvaloniaUI.ViewModels
{
    public class AuthWindowViewModel : ViewModelBase
    {

        DatabasePostgreSql database = new DatabasePostgreSql(
            host: ConfigurationManager.AppSettings["PostgreHost"],  
            database: ConfigurationManager.AppSettings["PostgreDatabase"],  
            username: ConfigurationManager.AppSettings["PostgreUsername"],
            password: ConfigurationManager.AppSettings["PostgrePassword"],
            port: Convert.ToInt32(ConfigurationManager.AppSettings["PostgrePort"]));

#pragma warning disable CA1822 // Mark members as static
        public string Greeting => "HELLO!";

#pragma warning restore CA1822 // Mark members as static
    }
}
