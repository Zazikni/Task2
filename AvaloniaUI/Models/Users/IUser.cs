namespace AvaloniaUI.Models.Users
{
    internal interface IUser
    {
        string Name { get; }
        string Password { get; }
        string Login { get; }
    }
}
