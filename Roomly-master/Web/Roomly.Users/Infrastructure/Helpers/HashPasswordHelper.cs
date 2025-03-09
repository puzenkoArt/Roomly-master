using System.Security.Cryptography;
using System.Text;

namespace Roomly.Users.Infrastructure.Helpers;

public static  class HashPasswordHelper
{
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hash;
        }
    }
}