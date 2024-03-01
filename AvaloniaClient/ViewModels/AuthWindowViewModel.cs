using AvaloniaClient.Models.AnswerManager;
using AvaloniaClient.Models.Backend;
using AvaloniaClient.Models.WindowManager;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using Serilog;
using System;
using System.Net.Sockets;
using System.Reactive;
using System.Threading.Tasks;
using System.Xml.Linq;
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
            _connection = ConnectionService.Instance.Client.Connected;
            ConnectionService.Instance.AddCallback(RefreshConnectionStatus);
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
            ServerResponse access ;
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
            try
            {
                
                //await _server.SendMessageAsync($"-auth {Login}@{Password}");
            }
            catch (SocketException ex)

            {
                //Task.Run(() => _server.Reconnect(RefreshConnectionStatus)); // если не удалось отправить - будет переподключаться к серверу.
                RefreshConnectionStatus(); // обновляет состояние окна пользовательского интерфейса
                Log.Debug($"Failure to send data to the server {ex.Message}");
                return;
            }
            try
            {
                access = new ServerResponse("404@asdasdasd");
                //access = await AnswerManager.AccessResponse( await _server.ReceiveMessageAsync()); // получает ответ от сервера и преобразовывает его
                Log.Debug($"Response.StatusCode =  {access.StatusCode} Response.Message =  {access.Message}");
            }
            catch (SocketException ex)
            {

                RefreshConnectionStatus(); // обновляет состояние окна пользовательского интерфейса

                Log.Debug($"Failure to receive data from the server {ex.Message}");
                return;
            }

            Login = String.Empty;
            Password = String.Empty;
            if( access.StatusCode == StatusCodes.OK )
            {
                ConnectionService.Instance.RemoveCallback(RefreshConnectionStatus);
                WindowManager.CloseRegWindow();
                WindowManager.SwitchToMainWindow();
                
            }
        }

        #endregion

        #region methods
        public async void RefreshConnectionStatus()
        {
            Log.Debug($"RefreshConnectionStatus ConnectionService.Instance.Client.Connected - {ConnectionService.Instance.Client.Connected}");
            Connection = ConnectionService.Instance.Client.Connected;

        }
        #endregion

    }
}
 