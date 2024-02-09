

namespace AvaloniaUI.Models.Users
{
    internal class NewUser : IUser
    {
        private string _name;
        private string _password;
        private string _login;
        public string Name { get { return _login; }}

        public string Password { get { return _password; } }

        public string Login { get { return _login; } }
        public NewUser(string name, string password, string login)
        {
            _name = name;
            _password = password;
            _login = login;
        }
    }

}
