using System.Text;

namespace Models.Security
{
    /// <summary>
    /// Класс для работы с хешем.
    /// </summary>
    internal static class PasswordHasher
    {
        #region methods

        /// <summary>
        /// Хеширует строку алгоритмом SHA-256.
        /// </summary>
        public static string CreateSHA256(string input)
        {
            using System.Security.Cryptography.SHA256 hash = System.Security.Cryptography.SHA256.Create();
            return Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(input)));
        }

        /// <summary>
        /// Хеширует строку алгоритмом SHA-256 и сверяет ее другой строкой.
        /// </summary>
        public static bool VerifyPassword(string password, string hash)
        {
            return hash.Equals(CreateSHA256(password));
        }

        #endregion methods
    }
}