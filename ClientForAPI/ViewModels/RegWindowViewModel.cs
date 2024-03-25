using ClientForAPI.Models.AnswerManager;
using ClientForAPI.Models.Backend;
using ClientForAPI.Models.WindowManager;
using ReactiveUI;
using Serilog;
using System;
using System.Reactive;

namespace ClientForAPI.ViewModels
{
    public class RegWindowViewModel : ViewModelBase
    {
        #region Fields

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

        #endregion Fields

        #region Commands

        public ReactiveCommand<Unit, Unit> RegUserCommand { get; }

        #endregion Commands

        #region Constructors

        public RegWindowViewModel()
        {
            Log.Debug($"Создание окна регистрации.");

            _connection = ConnectionService.Instance.Client.Connected;

            ConnectionService.Instance.AddCallback(RefreshConnectionStatus);
            RegUserCommand = ReactiveCommand.Create(RegUser);
        }

        #endregion Constructors

        #region CommMethods

        public async void RegUser()
        {
            Log.Debug($"Окно регистрации. Кнопка регистрации нажата.");
            Log.Debug($"Окно регистрации. TextBoxLogin: {Name}\tTextBoxLogin: {Login}\tTextBoxPassword:{Password}");

            ServerResponse response;
            ServerRequest request;

            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Login) || String.IsNullOrEmpty(Password))
            {
                Log.Information($"Окно регистрации. Данные не введены.");
                return;
            }
            if (String.IsNullOrWhiteSpace(Name) || String.IsNullOrWhiteSpace(Login) || String.IsNullOrWhiteSpace(Password))
            {
                Log.Information($"Окно регистрации. Данные не введены или состоят из пробелов.");
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
                Log.Information($"Окно регистрации. Запрос {request.Id} TimeoutException");
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

        #endregion CommMethods

        #region Methods

        public async void RefreshConnectionStatus()
        {
            Log.Debug($"Окно регистрации. Обновление статуса соеденения. Соеденение - {(ConnectionService.Instance.Client.Connected ? "Установлено" : "Потеряно")}");
            Connection = ConnectionService.Instance.Client.Connected;
        }

        #endregion Methods
    }
}