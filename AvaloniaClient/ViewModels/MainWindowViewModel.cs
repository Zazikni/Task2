using AvaloniaClient.Models.Backend;
using ReactiveUI;
using Serilog;
using System.Net.Sockets;
using System.Threading.Tasks;
using AvaloniaClient.Models.WindowManager;


namespace AvaloniaClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region fields
        private string _message = "Wait a bit...";
        private Server _server = Server.Instance;

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
            await _server.SendMessageAsync("-spam");
            while ( true )
            {
                try
                {
                    string result = await Task.Run(async () =>
                    {
                        string tmp = await _server.ReceiveMessageAsync();
                        return tmp;
                    });

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
