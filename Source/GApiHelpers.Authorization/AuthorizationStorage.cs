using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GApiHelpers.Authorization
{
    internal static class AuthorizationStorage
    {
        public static void SaveRefreshToken(string token, string filePath)
        {
            byte[] tokenData = Encoding.Unicode.GetBytes(token);
            byte[] encryptedData = ProtectedData.Protect(tokenData, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(filePath, encryptedData);
        }

        public static string LoadRefreshToken(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            byte[] encryptedData = File.ReadAllBytes(filePath);
            byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decryptedData);
        }

        public static void DeleteRefreshToken(string filePath)
        {
            File.Delete(filePath);
        }
    }
}