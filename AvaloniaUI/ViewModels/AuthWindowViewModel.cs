using AvaloniaUI.Models.Database;
using System.Configuration;
using System;
using ReactiveUI;
using Serilog;
using AvaloniaUI.Models.Security;
using System.Reactive;
using AvaloniaUI.Models.WindowManager;
namespace AvaloniaUI.ViewModels
{
    public class AuthWindowViewModel : ViewModelBase
    {

        DatabasePostgreSql database = DatabasePostgreSql.Instance;

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
        public ReactiveCommand<Unit, Unit> AuthUserCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenRegisterWindowCommand { get; }
        #endregion
        #region constructors
        public AuthWindowViewModel()
        {
            OpenRegisterWindowCommand = ReactiveCommand.Create(OpenRegisterWindow);
            AuthUserCommand = ReactiveCommand.Create(AuthUserByLogPass);
        }
        #endregion
        #region commMethods

        public async void OpenRegisterWindow()
        {

            Log.Debug($"Button with OpenRegisterWindowCommand was clicked!");
            WindowManager.ShowRegWindow();

        }

        public async void AuthUserByLogPass()
        {

            Log.Debug($"Button from AuthWindow with AuthUserCommand was clicked!");
            Log.Debug($"TextBoxLogin: {Login}\tTextBoxPassword:{Password}");
            bool access = await AuthenticationManager.AccessAllowed(login: Login, password: Password, database: database);
            Login = String.Empty;
            Password = String.Empty;
            if( access )
            {
                WindowManager.SwitchToMainWindow();
            }
        }
        #endregion

    }
}
 