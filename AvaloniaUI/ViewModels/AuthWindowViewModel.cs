using AvaloniaUI.Models.Database;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System;
using ReactiveUI;
using System.Windows.Input;
using Avalonia.Controls;
using Serilog;
using AvaloniaUI.Models.Security;
using System.Threading.Tasks;
using Avalonia;
using System.Reactive;
using System.Diagnostics;
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
        #region fields

        private string _login = string.Empty;
        private string _password = string.Empty;
        public string Login {
            get { return _login; }
            set { this.RaiseAndSetIfChanged(ref _login, value); }
        }
        public string Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        #endregion
        #region commands
        public ReactiveCommand<Unit, Unit> AuthUser { get; }
        public ReactiveCommand<Unit, Unit> DoSomeThing { get; }
        #endregion

        public AuthWindowViewModel()
        {
            DoSomeThing = ReactiveCommand.Create(RunSomeThing);
            AuthUser = ReactiveCommand.Create(AuthUserByLogPass);
        }
        #region commMethods
        public async void RunSomeThing()
        {
            Log.Debug("RunSomeThing START");
            await Task.Delay(10000);
            Log.Debug("RunSomeThing END");

        }
        public async void AuthUserByLogPass()
        {
            Login = String.Empty;
            Password = String.Empty;
            Log.Debug($"Button command was clicked!");
            Log.Debug($"TextBoxLogin: {Login} TextBoxPassword:{Password}");
            await Task.Delay(10000);
            bool access = await AuthenticationManager.AccessAllowed(login: Login, password: Password, database: database);

        }
        #endregion

#pragma warning restore CA1822 // Mark members as static
    }
    }
