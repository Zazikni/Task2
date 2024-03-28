using ClientForAPI.Models.AnswerManager;
using ClientForAPI.Models.RemoteServices;
using ClientForAPI.Models.WindowManager;
using ReactiveUI;
using Serilog;
using System;
using System.Reactive;
using System.Reactive.Linq;
namespace ClientForAPI.ViewModels
{
    public class AuthWindowViewModel : ViewModelBase
    {
        #region Fields

        private bool _connection;

        public bool Connection
        {
            get { return _connection; }
            set { this.RaiseAndSetIfChanged(ref _connection, value); }
        }
        private bool _contextFlag = true;
        public bool ContextFlag
        {
            get => _contextFlag;
            set => this.RaiseAndSetIfChanged(ref _contextFlag, value);
        }

        

        #region Auth
        private string _login_auth = string.Empty;
        private string _password_auth = string.Empty;

        public string LoginAuth
        {
            get { return _login_auth; }
            set { this.RaiseAndSetIfChanged(ref _login_auth, value); }
        }

        public string PasswordAuth
        {
            get { return _password_auth; }
            set { this.RaiseAndSetIfChanged(ref _password_auth, value); }
        }

        private string _response_info_auth = String.Empty;
        private bool _response_received_auth = false;
        public string ResponseInfoAuth
        {
            get => _response_info_auth;
            set => this.RaiseAndSetIfChanged(ref _response_info_auth, value);
        }

        public bool ResponseReceivedAuth
        {
            get => _response_received_auth;
            set => this.RaiseAndSetIfChanged(ref _response_received_auth, value);
        }

        #endregion Auth

        #region Reg
        private string _name_reg;
        private string _login_reg;
        private string _password_reg;
        private string _confirm_password_reg;
        private string _response_info_reg = String.Empty;
        private bool _response_received_reg = false;

        public string NameReg
        {
            get => _name_reg;
            set => this.RaiseAndSetIfChanged(ref _name_reg, value);
        }

        public string LoginReg
        {
            get => _login_reg;
            set => this.RaiseAndSetIfChanged(ref _login_reg, value);
        }

        public string PasswordReg
        {
            get => _password_reg;
            set => this.RaiseAndSetIfChanged(ref _password_reg, value);
        }
        public string ConfirmPasswordReg
        {
            get => _confirm_password_reg;
            set => this.RaiseAndSetIfChanged(ref _confirm_password_reg, value);
        }

        public string ResponseInfoReg
        {
            get => _response_info_reg;
            set => this.RaiseAndSetIfChanged(ref _response_info_reg, value);
        }


        public bool ResponseReceivedReg
        {
            get => _response_received_reg;
            set => this.RaiseAndSetIfChanged(ref _response_received_reg, value);
        }
        #endregion Reg



        #endregion Fields

        #region Commands


        #region Auth
        public ReactiveCommand<Unit, Unit> AuthUserCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenRegisterFormCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenAuthFormCommand { get; }

        #endregion Auth

        #region Reg

        public ReactiveCommand<Unit, Unit> RegUserCommand { get; }

        #endregion Reg



        #endregion Commands

        #region Constructors

        public AuthWindowViewModel()
        {
            Log.Debug($"Создание окна аутентификации.");

            _connection = AuthenticationService.Instance.Client.Connected;
            AuthenticationService.Instance.AddCallback(RefreshConnectionStatus);
            OpenRegisterFormCommand = ReactiveCommand.Create(OpenRegisterForm);
            OpenAuthFormCommand = ReactiveCommand.Create(OpenAuthForm);
            AuthUserCommand = ReactiveCommand.Create(AuthUserByLogPass);
            RegUserCommand = ReactiveCommand.Create(RegUser);

        }

        #endregion Constructors

        #region CommMethods

        #region Auth
        public async void OpenRegisterForm()
        {
            Log.Debug($"Окно аутентификации. Форма аутентификации. Кнопка открытия формы регистрации нажата.");
            ContextFlag = false;
        }

        public async void AuthUserByLogPass()
        {
            Log.Debug($"Окно аутентификации. Форма аутентификации. Кнопка аутентификации нажата.");
            Log.Debug($"Окно аутентификации. Форма аутентификации. TextBoxLogin: {LoginAuth}\tTextBoxPassword:{PasswordAuth}");
            ServerResponse response;
            ServerRequest request;
            if (String.IsNullOrEmpty(LoginAuth) || String.IsNullOrEmpty(PasswordAuth))
            {
                Log.Information($"Окно аутентификации. Форма аутентификации. Форма аутентификации. Данные не введены.");
                return;
            }
            if (String.IsNullOrWhiteSpace(LoginAuth) || String.IsNullOrWhiteSpace(PasswordAuth))
            {
                Log.Information($"Окно аутентификации. Форма аутентификации. Данные не введены или состоят из пробелов.");
                return;
            }
            request = new ServerRequest(command: "-auth", message: $"{LoginAuth}@{PasswordAuth}");
            AuthenticationService.Instance.AddRequest(request);
            try
            {
                response = await AuthenticationService.Instance.GetResponseAsync(response_id: request.Id, timeout: TimeSpan.FromSeconds(4));
            }
            catch (TimeoutException ex)
            {
                Log.Information($"Окно аутентификации. Форма аутентификации. Запрос {request.Id} TimeoutException");
                return;
            }

            LoginAuth = String.Empty;
            PasswordAuth = String.Empty;
            if (response.StatusCode == StatusCodes.OK)
            {
                AuthenticationService.Instance.RemoveCallback(RefreshConnectionStatus);
                WindowManager.CloseRegWindow();
                WindowManager.SwitchToMainWindow();
            }
            else
            {
                ResponseReceivedAuth = true;
                ResponseInfoAuth = response.Message;
            }
        }

        #endregion Auth

        #region Reg
        public async void RegUser()
        {
            Log.Debug($"Окно аутентификации. Форма регистрации. Кнопка регистрации нажата.");
            Log.Debug($"Окно аутентификации. Форма регистрации. TextBoxLogin: {NameReg}\tTextBoxLogin: {LoginReg}\tTextBoxPassword:{PasswordReg}");

            ServerResponse response;
            ServerRequest request;

            if (String.IsNullOrEmpty(NameReg) || String.IsNullOrEmpty(LoginReg) || String.IsNullOrEmpty(PasswordReg))
            {
                Log.Information($"Окно аутентификации. Форма регистрации. Данные не введены.");
                return;
            }
            if (String.IsNullOrWhiteSpace(NameReg) || String.IsNullOrWhiteSpace(LoginReg) || String.IsNullOrWhiteSpace(PasswordReg))
            {
                Log.Information($"Окно аутентификации. Форма регистрации. Данные не введены или состоят из пробелов.");
                return;
            }
            request = new ServerRequest(command: "-reg", message: $"{NameReg}@{LoginReg}@{PasswordReg}");
            AuthenticationService.Instance.AddRequest(request);
            try
            {
                response = await AuthenticationService.Instance.GetResponseAsync(response_id: request.Id, timeout: TimeSpan.FromSeconds(2));
            }
            catch (TimeoutException ex)
            {
                Log.Information($"Окно аутентификации. Форма регистрации. Запрос {request.Id} TimeoutException");
                return;
            }

            if (response.StatusCode == StatusCodes.CREATED)
            {
                Log.Information($"Окно аутентификации. Форма регистрации. Пользователь успешно создан, перенаправление на форму аутентификации.");
                LoginAuth = LoginReg;                
                ContextFlag = true;
                ClearRegFields();
            }
            else
            {
                ResponseReceivedReg = true;
                ResponseInfoReg = response.Message;
            }
        }
        public async void OpenAuthForm()
        {
            Log.Debug($"Окно аутентификации. Форма регистрации. Кнопка открытия формы аутентификации нажата.");
            ContextFlag = true;
        }

        #endregion Reg

        #endregion CommMethods

        #region Methods


        public async void RefreshConnectionStatus()
        {
            Log.Debug($"Окно аутентификации. Обновление статуса соеденения. Соеденение - {(AuthenticationService.Instance.Client.Connected ? "Установлено" : "Потеряно")}");
            Connection = AuthenticationService.Instance.Client.Connected;
        }

        public async void ClearRegFields()
        {
            Log.Debug($"Окно аутентификации. Форма регистрации. Очистка полей регистации.");
            NameReg = String.Empty;
            LoginReg = String.Empty;
            PasswordReg = String.Empty;
            ConfirmPasswordReg = String.Empty;

        }


        #endregion Methods
    }
}