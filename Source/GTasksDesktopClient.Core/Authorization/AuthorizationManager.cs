using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Tasks.v1;
using Google.Apis.Util;

namespace GTasksDesktopClient.Core.Authorization
{
    public static class AuthorizationManager
    {
        private const string AuthorizationResponseUrlFormat = "http://localhost:{0}/{1}/authorize/";

        private const string ClientIdentifier = "747950969211.apps.googleusercontent.com";
        private const string ClientSecret = "Pc30_p04vf01tF6apQ0bpDlS";

        public static event Action<Uri> AuthorizationRequired;
        public static event Action AuthorizationSucceeded;

        private static void TriggerAuthorizationRequired(Uri authorizationUrl)
        {
            var handler = AuthorizationRequired;
            if (handler != null)
                handler(authorizationUrl);
        }

        private static void TriggerAuthorizationSucceeded()
        {
            Action handler = AuthorizationSucceeded;
            if (handler != null)
                handler();
        }

        public static IAuthenticator GetAuthenticator()
        {
            var tokenProvider = new NativeApplicationClient(GoogleAuthenticationServer.Description)
                                    {
                                        ClientIdentifier = ClientIdentifier,
                                        ClientSecret = ClientSecret
                                    };

            return new OAuth2Authenticator<NativeApplicationClient>(tokenProvider, GetAuthorization);
        }

        private static IAuthorizationState GetAuthorization(NativeApplicationClient client)
        {
            var scope = TasksService.Scopes.Tasks.GetStringValue();
            var scopes = new[] { scope };

            var refreshToken = AuthorizationStorage.LoadRefreshToken();

            IAuthorizationState state = string.IsNullOrEmpty(refreshToken)
                                            ? ObtainCredentials(client, scopes)
                                            : RefreshCredentials(client, scopes, refreshToken);

            if (!string.IsNullOrEmpty(state.AccessToken))
                TriggerAuthorizationSucceeded();

            return state;
        }

        private static IAuthorizationState ObtainCredentials(NativeApplicationClient client, IEnumerable<string> scopes)
        {
            IAuthorizationState state = new AuthorizationState(scopes);
            var responseUrl = FormatResponseUrl();
            state.Callback = responseUrl;

            var authorizationResponseServer = new AuthorizationResponseServer(responseUrl);
            authorizationResponseServer.Start();

            var authorizationUrl = client.RequestUserAuthorization(state);
            TriggerAuthorizationRequired(authorizationUrl);

            string authorizationCode = authorizationResponseServer.GetAuthorizationCode();
            authorizationResponseServer.Stop();

            state = client.ProcessUserAuthorization(authorizationCode, state);
            AuthorizationStorage.SaveRefreshToken(state.RefreshToken);
            return state;
        }

        private static IAuthorizationState RefreshCredentials(NativeApplicationClient client, IEnumerable<string> scopes, string refreshToken)
        {
            IAuthorizationState state = new AuthorizationState(scopes) { RefreshToken = refreshToken };

            try
            {
                client.RefreshToken(state);
            }
            catch
            {
                state = ObtainCredentials(client, scopes);
            }

            return state;
        }

        private static Uri FormatResponseUrl()
        {
            var applicationNAme = Assembly.GetEntryAssembly().GetName().Name;
            var port = GetRandomUnusedPort();
            string url = string.Format(AuthorizationResponseUrlFormat, port, applicationNAme);
            return new Uri(url);
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            try
            {
                listener.Start();
                return ((IPEndPoint)listener.LocalEndpoint).Port;
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}