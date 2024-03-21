using AvaloniaClient.Models.AnswerManager;
using AvaloniaClient.Models.Backend;
using AvaloniaClient.Models.WindowManager;
using ReactiveUI;
using Serilog;
using System;
using System.Net.Sockets;
using System.Reactive;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tmds.DBus.Protocol;

namespace AvaloniaClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private string _message = "Wait a bit...";
        private bool _connection;

        public bool Connection
        {
            get { return _connection; }
            set { this.RaiseAndSetIfChanged(ref _connection, value); }
        }

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        #endregion Fields

        #region Commands
        public ReactiveCommand<Unit, Unit> SwitchToAuthWindowCommand { get; }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            Log.Debug($"Создание главного окна.");
            _connection = ConnectionService.Instance.Client.Connected;

            ConnectionService.Instance.AddCallback(RefreshConnectionStatus);
            SwitchToAuthWindowCommand = ReactiveCommand.Create(SwitchToAuthWindow);


            GetDataFromServerAsync();
        }

        #endregion Constructors

        #region Tasks

        private async Task GetDataFromServerAsync()
        {
            await ServerSpam();


            while (true)
            {
                try
                {
                    string result = await Task.Run(async () =>
                    {
                        ServerResponse response = await ConnectionService.Instance.GetResponseAsync(response_id: 0000, timeout: TimeSpan.FromSeconds(20));
                        return response.Message;
                    }
                    );

                    Message += "\n" + result;
                }
                catch (SocketException ex)
                {
                    Log.Error($"GetDataFromServerAsync {ex.Message}");
                    WindowManager.SwitchToAuthWindow();
                    break;
                }

            }
            Log.Information($"Получение сообщений из рассылки сервера - закончено.");
        }

        #endregion Tasks

        #region Methods
        public async void RefreshConnectionStatus()
        {
            Log.Debug($"Главное окно. Обновление статуса соеденения. Соеденение - {(ConnectionService.Instance.Client.Connected ? "Установлено" : "Потеряно")}");

            //Connection = ConnectionService.Instance.Client.Connected;
            Connection = false;

        }
        public async Task ServerSpam()
        {
            Log.Information($"Получение сообщений из рассылки сервера - запущено.");

            ServerRequest request = new ServerRequest(command: "-spam");
            ConnectionService.Instance.AddRequest(request);
            ServerResponse comm_response = await ConnectionService.Instance.GetResponseAsync(response_id: request.Id, timeout: TimeSpan.FromSeconds(2));
        }

        #endregion Methods

        #region CommMethods

        public async void SwitchToAuthWindow()
        {
            Log.Debug($"Главное окно. Кнопка возврата к авторизации нажата.");
            WindowManager.SwitchToAuthWindow();

        }
        #endregion
    }
}