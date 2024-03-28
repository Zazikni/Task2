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
        private FileDialogService? FileDialogServiceInstance;


        private string _message = "Покажу ниже путь к фото.";
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
        private async Task InitFileDialogServiceInstanceIfNotExists()
        {
            if (FileDialogServiceInstance == null)
            {
                Log.Debug($"Главное окно. Сервис выбора файлов не инициализирован.");
                FileDialogServiceInstance = new FileDialogService(WindowManagerService.Instance.GetMainWindow);
                Log.Debug($"Главное окно. Сервис выбора файлов успешно инициализирован.");

                return;
            }
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
            await InitFileDialogServiceInstanceIfNotExists();

            FileDialogFilter filter = new FileDialogFilter { Name = "Image Files", Extensions = new List<string> { "png", "jpg", "jpeg" } };
            string[] files_to_send = await FileDialogServiceInstance.ShowOpenFileDialogAsync(filter: filter);

            foreach ( string file in files_to_send)
            {
                Log.Debug($"Пользователь выбрал файл: {file}");
                Message += "\n" + file;
            }

        }
        #endregion
    }
}