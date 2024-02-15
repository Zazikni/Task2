using AvaloniaUI.Models.Database;
using AvaloniaUI.Models.WindowManager;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _message;

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        public MainWindowViewModel()
        {
            Message = "Hello from the new window!";
        }
        #region fields
        #endregion
        #region commands
        #endregion
        #region constructors
        #endregion
        #region commMethods

        #endregion
    }
}
