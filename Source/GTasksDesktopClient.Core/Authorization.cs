using GApiHelpers.Authorization;
using Google.Apis.Tasks.v1;
using Google.Apis.Util;

namespace GTasksDesktopClient.Core
{
    public static class Authorization
    {
        private const string ClientIdentifier = "471999494130.apps.googleusercontent.com";
        private const string ClientSecret = "vRG66n9Q3zHWCEa6UqAQ80E6";
        private const string RefreshTokenFilePath = "google-tasks-auth.token";

        public static AuthorizationConfig GetConfiguration()
        {
            var scope = TasksService.Scopes.Tasks.GetStringValue();
            var scopes = new[] { scope };

            var authorizationConfig = new AuthorizationConfig
            {
                ClientIdentifier = ClientIdentifier,
                ClientSecret = ClientSecret,
                Scopes = scopes,
                RefreshTokenFilePath = RefreshTokenFilePath
            };

            return authorizationConfig;
        }
    }
}