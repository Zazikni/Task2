namespace Models.Users
{
    internal interface IUser
    {
        #region fields

        string Name { get; }
        string Password { get; }
        string Login { get; }

        #endregion fields
    }
}