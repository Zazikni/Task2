using Models.Users;
using System.Threading.Tasks;

namespace Models.Database
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
