
using AvaloniaClient.Models.AnswerManager;
using AvaloniaClient.Models.Backend;
using AvaloniaClient.Models.WindowManager;
using ReactiveUI;
using Serilog;
using System;
using System.Net.Sockets;
using System.Reactive;
using System.Threading.Tasks;

namespace AvaloniaClient.ViewModels
{
    public class RegWindowViewModel : ViewModelBase
    {

        #region fields
        private Server _server = Server.Instance;

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
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> RegUserCommand { get; }
        #endregion

        #region constructors
        public RegWindowViewModel()
        {
            RegUserCommand = ReactiveCommand.Create(RegUser);
        }
        #endregion

        #region commMethods
        public async void RegUser()
        {

            Log.Debug($"Button from RegWindow with RegUserCommand was clicked!");
            Log.Debug($"TextBoxLogin: {Name}\tTextBoxLogin: {Login}\tTextBoxPassword:{Password}");

            ServerResponse response;
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
            try
            {

                await _server.SendMessageAsync($"-reg {Name}@{Login}@{Password}");
            }
            catch (SocketException ex)

            {
                Task.Run(() => _server.Reconnect(RefreshConnectionStatus)); // если не удалось отправить - будет переподключаться к серверу.
                RefreshConnectionStatus(); // обновляет состояние окна пользовательского интерфейса
                Log.Debug($"Failure to send data to the server {ex.Message}");
                return;
            }
            try
            {
                response = await AnswerManager.RegResponse(await _server.ReceiveMessageAsync()); // получает ответ от сервера и преобразовывает его
                //Log.Debug($"Access =  {access}");
            }
            catch (SocketException ex)
            {

                RefreshConnectionStatus(); // обновляет состояние окна пользовательского интерфейса

                Log.Debug($"Failure to send data to the server {ex.Message}");
                return;
            }


            if (response.StatusCode == StatusCodes.CREATED)
            {
                WindowManager.CloseRegWindow();

            }
            else
            {
                ResponseReceived = true;
                ResponseInfo = response.Message;
            }
        }
        #endregion
        #region methods
        public async void RefreshConnectionStatus()
        {
            Log.Debug($"RefreshConnectionStatus _server.Connected - {_server.Client.Connected}");
            Connection = _server.Client.Connected;

        }
        #endregion
    }
}
