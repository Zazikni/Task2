using Events;
using ReactiveUI;
using Serilog;
using System.Threading.Tasks;

namespace AvaloniaUI.ViewModels
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

            CalculateMessageAsync();

        }
        #endregion

        #region tasks
        private async Task CalculateMessageAsync()
        {
            Log.Debug($"CalculateMessageAsync start");
            string result = await Task.Run(() =>
            {
                Log.Debug($"Calculation start");
                string tmp = EventsProgram.Run();
                Log.Debug($"Calculation end");
                return tmp;
            });
            Log.Debug($"CalculateMessageAsync finish");

            Message = result;
        }

        #endregion

    }

}
