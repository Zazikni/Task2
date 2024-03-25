using ClientForAPI.Models.AnswerManager;
using ClientForAPI.Models.Backend;
using ClientForAPI.Models.WindowManager;
using ReactiveUI;
using Serilog;
using System;
using System.Reactive;

namespace ClientForAPI.ViewModels
{
    public class AuthWindowViewModel : ViewModelBase
    {
        #region Fields

        private bool _connection;

        public bool Connection
        {
            get { return _connection; }
            set { this.RaiseAndSetIfChanged(ref _connection, value); }
        }

        private string _login = string.Empty;
        private string _password = string.Empty;

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
        private string _response_info = String.Empty;
        private bool _response_received = false;
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

        #endregion Fields

        #region Commands

        public ReactiveCommand<Unit, Unit> AuthUserCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenRegisterWindowCommand { get; }

        #endregion Commands

        #region Constructors

        public AuthWindowViewModel()
        {
            Log.Debug($"Создание окна аутентификации.");

            _connection = ConnectionService.Instance.Client.Connected;
            ConnectionService.Instance.AddCallback(RefreshConnectionStatus);
            OpenRegisterWindowCommand = ReactiveCommand.Create(OpenRegisterWindow);
            AuthUserCommand = ReactiveCommand.Create(AuthUserByLogPass);
        }

        #endregion Constructors

        #region CommMethods

        public async void OpenRegisterWindow()
        {
            Log.Debug($"Окно аутентификации. Кнопка открытия окна регистрации нажата.");
            WindowManager.ShowRegWindow();
        }

        public async void AuthUserByLogPass()
        {
            Log.Debug($"Окно аутентификации. Кнопка аутентификации нажата.");
            Log.Debug($"Окно аутентификации. TextBoxLogin: {Login}\tTextBoxPassword:{Password}");
            ServerResponse response;
            ServerRequest request;
            if (String.IsNullOrEmpty(Login) || String.IsNullOrEmpty(Password))
            {
                Log.Information($"Окно аутентификации. Данные не введены.");
                return;
            }
            if (String.IsNullOrWhiteSpace(Login) || String.IsNullOrWhiteSpace(Password))
            {
                Log.Information($"Окно аутентификации. Данные не введены или состоят из пробелов.");
                return;
            }
            request = new ServerRequest(command: "-auth", message: $"{Login}@{Password}");
            ConnectionService.Instance.AddRequest(request);
            try
            {
                response = await ConnectionService.Instance.GetResponseAsync(response_id: request.Id, timeout: TimeSpan.FromSeconds(4));
            }
            catch (TimeoutException ex)
            {
                Log.Information($"Окно аутентификации. Запрос {request.Id} TimeoutException");
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
            else
            {
                ResponseReceived = true;
                ResponseInfo = response.Message;
            }
        }

        #endregion CommMethods

        #region Methods

        public async void RefreshConnectionStatus()
        {
            Log.Debug($"Окно аутентификации. Обновление статуса соеденения. Соеденение - {(ConnectionService.Instance.Client.Connected ? "Установлено" : "Потеряно")}");
            Connection = ConnectionService.Instance.Client.Connected;
        }


        #endregion Methods
    }
}