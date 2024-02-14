using AvaloniaUI.Models.Users;
using System.Threading.Tasks;

namespace AvaloniaUI.Models.Database
{
    internal interface IDatabase
    {
        #region fields
        #endregion

        #region constructors

        #endregion

        #region methods
        public  Task<User?> GetUser(string login);
        public Task AddUser(NewUser user);
        #endregion

    }
}
