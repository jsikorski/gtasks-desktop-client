using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;

namespace GApiHelpers.Authorization
{
    public class AuthorizationManager
    {
        private const string AuthorizationResponseUrlFormat = "http://localhost:{0}/{1}/authorize/";

        private static string _clientIdentifier;
        private static string _clientSecret;
        private static IEnumerable<string> _scopes;
        private static string _refreshTokenFilePath;

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

        public static void Initialize(AuthorizationConfig authorizationConfig)
        {
            authorizationConfig.Validate();

            _clientIdentifier = authorizationConfig.ClientIdentifier;
            _clientSecret = authorizationConfig.ClientSecret;
            _scopes = authorizationConfig.Scopes;
            _refreshTokenFilePath = authorizationConfig.RefreshTokenFilePath;
        }

        public static IAuthenticator GetAuthenticator()
        {
            var tokenProvider = new NativeApplicationClient(GoogleAuthenticationServer.Description)
                                    {
                                        ClientIdentifier = _clientIdentifier,
                                        ClientSecret = _clientSecret
                                    };

            return new OAuth2Authenticator<NativeApplicationClient>(tokenProvider, GetAuthorization);
        }

        private static IAuthorizationState GetAuthorization(NativeApplicationClient client)
        {
            var refreshToken = AuthorizationStorage.LoadRefreshToken(_refreshTokenFilePath);

            IAuthorizationState state = string.IsNullOrEmpty(refreshToken)
                                            ? ObtainCredentials(client)
                                            : RefreshCredentials(client, refreshToken);

            if (!string.IsNullOrEmpty(state.AccessToken))
                TriggerAuthorizationSucceeded();

            return state;
        }

        private static IAuthorizationState ObtainCredentials(NativeApplicationClient client)
        {
            IAuthorizationState state = new AuthorizationState(_scopes);
            var responseUrl = FormatResponseUrl();
            state.Callback = responseUrl;

            var authorizationResponseServer = new AuthorizationResponseServer(responseUrl);
            authorizationResponseServer.Start();

            var authorizationUrl = client.RequestUserAuthorization(state);
            TriggerAuthorizationRequired(authorizationUrl);

            string authorizationCode = authorizationResponseServer.GetAuthorizationCode();
            authorizationResponseServer.Stop();

            state = client.ProcessUserAuthorization(authorizationCode, state);
            AuthorizationStorage.SaveRefreshToken(state.RefreshToken, _refreshTokenFilePath);
            return state;
        }

        private static IAuthorizationState RefreshCredentials(NativeApplicationClient client, string refreshToken)
        {
            IAuthorizationState state = new AuthorizationState(_scopes) { RefreshToken = refreshToken };

            try
            {
                client.RefreshToken(state);
            }
            catch
            {
                state = ObtainCredentials(client);
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