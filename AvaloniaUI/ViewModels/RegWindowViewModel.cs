using AvaloniaUI.Models.Users;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaUI.ViewModels
{
    public class RegWindowViewModel : ViewModelBase
    {



        #region fields
        private string _message;
        private string _name;
        private string _login;
        private string _password;
        

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
        #endregion
        #region commands
        public ReactiveCommand<Unit, Unit> RegUserCommand { get; }
        #endregion
        #region constructors
        public RegWindowViewModel()
        {
            Message = "Hello from the new Reg window!";
            RegUserCommand = ReactiveCommand.Create(RegUser);
        }
        #endregion
        #region commMethods
        public async void RegUser()
        {

            Log.Debug($"Button from RegWindow with RegUserCommand was clicked!");
            Log.Debug($"TextBoxLogin: {Name}\tTextBoxLogin: {Login}\tTextBoxPassword:{Password}");

        }
        #endregion
    }
}
