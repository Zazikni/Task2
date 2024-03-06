using AvaloniaClient.Models.AnswerManager;
using AvaloniaClient.Models.Backend;
using AvaloniaClient.Models.WindowManager;
using ReactiveUI;
using Serilog;
using System;
using System.Reactive;

namespace AvaloniaClient.ViewModels
{
    public class AuthWindowViewModel : ViewModelBase
    {
        #region fields

        //private Server _server = Server.Instance;
        private bool _connection;

        public bool Connection
        {
            get { return _connection; }
            set { this.RaiseAndSetIfChanged(ref _connection, value); }
        }

        private string _login = string.Empty;
        private string _password = string.Empty;
        //IDatabase database = DatabasePostgreSql.Instance;

        public string Login
        {
            get { return _login; }
            set { this.RaiseAndSetIfChanged(ref _login, value); }
        }

        public string Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        #endregion fields

        #region commands

        public ReactiveCommand<Unit, Unit> AuthUserCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenRegisterWindowCommand { get; }

        #endregion commands

        #region constructors

        public AuthWindowViewModel()
        {
            _connection = ConnectionService.Instance.Client.Connected;
            ConnectionService.Instance.AddCallback(RefreshConnectionStatus);
            OpenRegisterWindowCommand = ReactiveCommand.Create(OpenRegisterWindow);
            AuthUserCommand = ReactiveCommand.Create(AuthUserByLogPass);
        }

        #endregion constructors

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
            ServerResponse response;
            ServerRequest request;
            if (String.IsNullOrEmpty(Login) || String.IsNullOrEmpty(Password))
            {
                Log.Information($"Incorrect data.");
                return;
            }
            if (String.IsNullOrWhiteSpace(Login) || String.IsNullOrWhiteSpace(Password))
            {
                Log.Information($"Incorrect data.");
                return;
            }
            request = new ServerRequest(command: "-auth", message: $"{Login}@{Password}");
            ConnectionService.Instance.AddRequest(request);
            try
            {
                response = await ConnectionService.Instance.GetResponseAsync(response_id: request.Id, timeout: TimeSpan.FromSeconds(2));
            }
            catch (TimeoutException ex)
            {
                Log.Information($"Запрос {request.Id} TimeoutException");
                return;
            }

            Login = String.Empty;
            Password = String.Empty;
            if (response.StatusCode == StatusCodes.OK)
            {
                ConnectionService.Instance.RemoveCallback(RefreshConnectionStatus);
                WindowManager.CloseRegWindow();
                WindowManager.SwitchToMainWindow();
            }
        }

        #endregion commMethods

        #region methods

        public async void RefreshConnectionStatus()
        {
            Log.Debug($"RefreshConnectionStatus ConnectionService.Instance.Client.Connected - {ConnectionService.Instance.Client.Connected}");
            Connection = ConnectionService.Instance.Client.Connected;
        }

        #endregion methods
    }
}