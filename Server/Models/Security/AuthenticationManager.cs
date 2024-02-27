using Models.Database;
using Models.Users;
using Serilog;
using System.Threading.Tasks;

namespace Models.Security
{
    /// <summary>
    /// Класс для аутентификации пользователей.
    /// </summary>
    internal static class AuthenticationManager
    {
        #region methods
        /// <summary>
        /// Проверяет имеет ли пользователь доступ.
        /// </summary>

        public static async Task<bool> AccessAllowed(string login, string password, IDatabase database)
        {
            User? user =  await database.GetUser(login:login);
            if (user == null)
            {
                Log.Information($"User with login: {login} not found.");
                return false;
            }
            else
            {
                if (PasswordHasher.VerifyPassword(password:password, hash:user.Password))
                {
                    Log.Information($"User with login: {login} access allowed.");
                    return true;

                }
                else
                {
                    Log.Information($"User with login: {login} access denied.");
                    return false;
                }
            }
        }
        #endregion
    }
}
