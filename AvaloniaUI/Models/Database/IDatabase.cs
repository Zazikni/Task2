using AvaloniaUI.Models.Users;

namespace AvaloniaUI.Models.Database
{
    internal interface IDatabase
    {
        #region fields
        #endregion

        #region constructors

        #endregion

        #region methods
        public User? GetUser(string login) { return null; }
        public void AddUser(NewUser user);
        #endregion

    }
}
