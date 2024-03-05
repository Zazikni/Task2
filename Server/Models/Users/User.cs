namespace Models.Users
{
    internal class User : NewUser
    {
        #region fields

        private Int64 _id;
        private Int64 Id
        { get { return _id; } }

        #endregion fields

        #region constructors

        public User(string name, string password, string login, Int64 id) : base(name, password, login)
        {
            _id = id;
        }

        #endregion constructors
    }
}