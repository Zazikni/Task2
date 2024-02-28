using AvaloniaClient.Models.Backend;
using ReactiveUI;
using Serilog;
using System.Net.Sockets;
using System.Threading.Tasks;

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

            GetDataAsync();

        }
        #endregion

        #region tasks
        private async Task GetDataAsync()
        {
            Log.Debug($"GetDataAsync start");
            await _server.SendMessageAsync("-spam");
            while ( true )
            {
                try
                {
                    string result = await Task.Run(async () =>
                    {
                        Log.Debug($"Calculation start");
                        string tmp = await _server.ReceiveMessageAsync();
                        Log.Debug($"Calculation end");
                        return tmp;
                    });

                    Message = result;
                    await Task.Delay(10000);

                }
                catch( SocketException ex)
                {
                    Log.Error($"GetDataAsync {ex.Message}");

                }


            }



        }

        #endregion

    }

}
