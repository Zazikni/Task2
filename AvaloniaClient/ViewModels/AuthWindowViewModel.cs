using System.Configuration;
using System;
using ReactiveUI;
using Serilog;
using System.Reactive;
using AvaloniaClient.Models.WindowManager;
using AvaloniaClient.Models.Backend;
using System.Net.Sockets;
using AvaloniaClient.Models.AnswerManager;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
namespace AvaloniaClient.ViewModels
{
    public class AuthWindowViewModel : ViewModelBase
    {

        #region fields
        private Server _server = Server.Instance;
        private bool _connection;
        private string _login = string.Empty;
        private string _password = string.Empty;
        //IDatabase database = DatabasePostgreSql.Instance;

        public bool Connection
        {
            get { return _connection; }
            set { this.RaiseAndSetIfChanged(ref _connection, value); }

        }

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
            _connection = _server.Client.Connected;
            OpenRegisterWindowCommand = ReactiveCommand.Create(OpenRegisterWindow);
            AuthUserCommand = ReactiveCommand.Create(AuthUserByLogPass);
            if (!Connection)
            {
                Task.Run(() => _server.Reconnect(RefreshConnectionStatus));

            }
        }
        #endregion

        #region commMethods

        public async void OpenRegisterWindow()
        {
            RefreshConnectionStatus();

            Log.Debug($"Button with OpenRegisterWindowCommand was clicked!");
            //WindowManager.ShowRegWindow();

        }


        public async void AuthUserByLogPass()
        {
            Log.Debug($"Button from AuthWindow with AuthUserCommand was clicked!");
            Log.Debug($"TextBoxLogin: {Login}\tTextBoxPassword:{Password}");
            bool access = false;//await AuthenticationManager.AccessAllowed(login: Login, password: Password, database: database);
            try
            {
                await _server.SendMessageAsync($"-auth {Login}@{Password}");
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
                access = await AnswerManager.Access( await _server.ReceiveMessageAsync()); // получает ответ от сервера и преобразовывает его
                Log.Debug($"Access =  {access}");
            }
            catch (SocketException ex)
            {

                RefreshConnectionStatus(); // обновляет состояние окна пользовательского интерфейса

                Log.Debug($"Failure to send data to the server {ex.Message}");
                return;
            }

            Login = String.Empty;
            Password = String.Empty;
            if( access )
            {
                //WindowManager.CloseRegWindow();
                WindowManager.SwitchToMainWindow();
                
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
 