using AvaloniaClient.Models.AnswerManager;
using AvaloniaClient.Models.Backend;
using AvaloniaClient.Models.WindowManager;
using ReactiveUI;
using Serilog;
using System;
using System.Reactive;

namespace AvaloniaClient.ViewModels
{
    public class RegWindowViewModel : ViewModelBase
    {
        #region fields

        private string _name;
        private string _login;
        private string _password;
        private string _response_info = String.Empty;
        private bool _response_received = false;
        private bool _connection;

        public bool Connection
        {
            get { return _connection; }
            set { this.RaiseAndSetIfChanged(ref _connection, value); }
        }

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string ResponseInfo
        {
            get => _response_info;
            set => this.RaiseAndSetIfChanged(ref _response_info, value);
        }

        public bool ResponseReceived
        {
            get => _response_received;
            set => this.RaiseAndSetIfChanged(ref _response_received, value);
        }

        #endregion fields

        #region commands

        public ReactiveCommand<Unit, Unit> RegUserCommand { get; }

        #endregion commands

        #region constructors

        public RegWindowViewModel()
        {
            _connection = ConnectionService.Instance.Client.Connected;

            ConnectionService.Instance.AddCallback(RefreshConnectionStatus);
            RegUserCommand = ReactiveCommand.Create(RegUser);
        }

        #endregion constructors

        #region commMethods

        public async void RegUser()
        {
            Log.Debug($"Button from RegWindow with RegUserCommand was clicked!");
            Log.Debug($"TextBoxLogin: {Name}\tTextBoxLogin: {Login}\tTextBoxPassword:{Password}");

            ServerResponse response;
            ServerRequest request;

            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Login) || String.IsNullOrEmpty(Password))
            {
                Log.Information($"Incorrect data.");
                return;
            }
            if (String.IsNullOrWhiteSpace(Name) || String.IsNullOrWhiteSpace(Login) || String.IsNullOrWhiteSpace(Password))
            {
                Log.Information($"Incorrect data.");
                return;
            }
            request = new ServerRequest(command: "-reg", message: $"{Name}@{Login}@{Password}");
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

            if (response.StatusCode == StatusCodes.CREATED)
            {
                ConnectionService.Instance.RemoveCallback(RefreshConnectionStatus);
                //TODO при удалении окна программно возможно программа упадет так как в делегате отснется ссылка на этот метод
                WindowManager.CloseRegWindow();
            }
            else
            {
                ResponseReceived = true;
                ResponseInfo = response.Message;
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