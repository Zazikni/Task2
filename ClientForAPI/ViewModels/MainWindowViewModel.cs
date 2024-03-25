using ClientForAPI.Models.AnswerManager;
using ClientForAPI.Models.Backend;
using ClientForAPI.Models.WindowManager;
using ReactiveUI;
using Serilog;
using System;
using System.Net.Sockets;
using System.Reactive;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tmds.DBus.Protocol;

namespace ClientForAPI.ViewModels
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


        }

        #endregion Constructors

        #region Tasks

        
        #endregion Tasks

        #region Methods
        public async void RefreshConnectionStatus()
        {
            Log.Debug($"Главное окно. Обновление статуса соеденения. Соеденение - {(ConnectionService.Instance.Client.Connected ? "Установлено" : "Потеряно")}");

            //Connection = ConnectionService.Instance.Client.Connected;
            Connection = false;

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