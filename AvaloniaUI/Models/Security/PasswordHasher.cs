using System;
using System.Text;

namespace AvaloniaUI.Models.Security
{
    internal static class PasswordHasher
    {
        public static string CreateSHA256(string input)
        {
            using System.Security.Cryptography.SHA256 hash = System.Security.Cryptography.SHA256.Create();
            return Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(input)));
        }
        public static bool VerifyPassword(string password, string hash)
        {
            return hash.Equals(CreateSHA256(password));
        }
    }
}
