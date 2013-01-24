using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GTasksDesktopClient.Core.Authorization
{
    public static class AuthorizationStorage
    {
        private const string RefreshTokenFileName = "google-tasks.token";

        public static void SaveRefreshToken(string token)
        {
            byte[] tokenData = Encoding.Unicode.GetBytes(token);
            byte[] encryptedData = ProtectedData.Protect(tokenData, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(RefreshTokenFileName, encryptedData);
        }

        public static string LoadRefreshToken()
        {
            if (!File.Exists(RefreshTokenFileName))
                return null;

            byte[] encryptedData = File.ReadAllBytes(RefreshTokenFileName);
            byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decryptedData);
        }
    }
}