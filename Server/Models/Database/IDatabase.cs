using Models.Users;

namespace Models.Database
{
    internal interface IDatabase
    {
        #region methods

        public Task<User?> GetUser(string login);

        public Task AddUser(NewUser user);

        #endregion methods
    }
}