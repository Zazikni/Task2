using AvaloniaClient.Models.AnswerManager;
using AvaloniaClient.Models.Backend;
using AvaloniaClient.Models.WindowManager;
using ReactiveUI;
using Serilog;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AvaloniaClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private string _message = "Wait a bit...";

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        #endregion Fields

        #region Constructors

        public MainWindowViewModel()
        {
            Log.Debug($"Создание главного окна.");

            GetDataFromServerAsync();
        }

        #endregion Constructors

        #region Tasks

        private async Task GetDataFromServerAsync()
        {
            Log.Information($"Получение сообщений из рассылки сервера - запущено.");

            ServerRequest request = new ServerRequest(command: "-spam");
            ConnectionService.Instance.AddRequest(request);
            ServerResponse comm_response = await ConnectionService.Instance.GetResponseAsync(response_id: request.Id, timeout: TimeSpan.FromSeconds(2));

            while (true)
            {
                try
                {
                    string result = await Task.Run(async () =>
                    {
                        ServerResponse response = await ConnectionService.Instance.GetResponseAsync(response_id: 0000, timeout: TimeSpan.FromSeconds(20000));
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
    }
}