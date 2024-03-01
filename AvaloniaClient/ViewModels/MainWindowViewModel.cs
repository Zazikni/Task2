using AvaloniaClient.Models.Backend;
using ReactiveUI;
using Serilog;
using System.Net.Sockets;
using System.Threading.Tasks;
using AvaloniaClient.Models.WindowManager;
using System;
using AvaloniaClient.Models.AnswerManager;


namespace AvaloniaClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region fields
        private string _message = "Wait a bit...";

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }
        #endregion

        #region constructors
        public MainWindowViewModel()
        {
            Log.Debug($"MainWindowViewModel init");

            GetDataFromServerAsync();

        }
        #endregion

        #region tasks
        private async Task GetDataFromServerAsync()
        {
            Log.Information($"GetDataFromServerAsync start");

            ServerRequest request = new ServerRequest(command:"-spam");
            ConnectionService.Instance.AddRequest(request);
            ServerResponse comm_response = await ConnectionService.Instance.GetResponseAsync(response_id: request.Id, timeout: TimeSpan.FromSeconds(2));

            while ( true )
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
                catch( SocketException ex)
                {
                    Log.Error($"GetDataAsync {ex.Message}");
                    WindowManager.SwitchToAuthWindow();
                    break;
                }


            }
            Log.Information($"GetDataFromServerAsync end");



        }

        #endregion

    }

}
