using ClientForAPI.Models.RemoteServices;
using ClientForAPI.Models.LocalServices;
using ReactiveUI;
using Serilog;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
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
        public ReactiveCommand<Unit, Unit> LoadFileCommand { get; }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            Log.Debug($"Создание главного окна.");
            _connection = AuthenticationService.Instance.Client.Connected;

            AuthenticationService.Instance.AddCallback(RefreshConnectionStatus);
            SwitchToAuthWindowCommand = ReactiveCommand.Create(SwitchToAuthWindow);
            LoadFileCommand = ReactiveCommand.Create(LoadFile);


        }

        #endregion Constructors

        #region Tasks

        
        #endregion Tasks

        #region Methods
        public async void RefreshConnectionStatus()
        {
            Log.Debug($"Главное окно. Обновление статуса соеденения. Соеденение - {(AuthenticationService.Instance.Client.Connected ? "Установлено" : "Потеряно")}");

            //Connection = ConnectionService.Instance.Client.Connected;
            Connection = false;

        }
        public async void OpenFileDialogTask()
        {

        }
        #endregion Methods

        #region CommMethods

        public async void SwitchToAuthWindow()
        {
            Log.Debug($"Главное окно. Кнопка возврата к авторизации нажата.");
            WindowManagerService.Instance.SwitchToAuthWindow();

        }
        public async void LoadFile()
        {
            Log.Debug($"Главное окно. Кнопка выбора изображения нажата.");

        }
        #endregion
    }
}