﻿using System;
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

        private readonly string _clientIdentifier;
        private readonly string _clientSecret;
        private readonly IEnumerable<string> _scopes;
        private readonly string _refreshTokenFilePath;

        public event Action<Uri> AuthorizationRequired;
        public event Action AuthorizationSucceeded;
        public event Action AuthorizationCanceled;
        public event Action AuthorizationNotSupported;

        private static void TriggerActionHandler(Action handler)
        {
            if (handler != null)
                handler();
        }

        private static void TriggerActionHandler<T>(Action<T> handler, T parameter)
        {
            if (handler != null)
                handler(parameter);
        }

        public AuthorizationManager(AuthorizationConfig authorizationConfig)
        {
            authorizationConfig.Validate();

            _clientIdentifier = authorizationConfig.ClientIdentifier;
            _clientSecret = authorizationConfig.ClientSecret;
            _scopes = authorizationConfig.Scopes;
            _refreshTokenFilePath = authorizationConfig.RefreshTokenFilePath;
        }

        public IAuthenticator GetAuthenticator()
        {
            var tokenProvider = new NativeApplicationClient(GoogleAuthenticationServer.Description)
                                    {
                                        ClientIdentifier = _clientIdentifier,
                                        ClientSecret = _clientSecret
                                    };

            return new OAuth2Authenticator<NativeApplicationClient>(tokenProvider, GetAuthorization);
        }

        private IAuthorizationState GetAuthorization(NativeApplicationClient client)
        {
            var refreshToken = AuthorizationStorage.LoadRefreshToken(_refreshTokenFilePath);

            IAuthorizationState state = string.IsNullOrEmpty(refreshToken)
                                            ? ObtainCredentials(client)
                                            : RefreshCredentials(client, refreshToken);

            if (!string.IsNullOrEmpty(state.AccessToken))
                TriggerActionHandler(AuthorizationSucceeded);

            return state;
        }

        private IAuthorizationState ObtainCredentials(NativeApplicationClient client)
        {
            IAuthorizationState state = new AuthorizationState(_scopes);
            var responseUrl = FormatResponseUrl();
            state.Callback = responseUrl;

            var authorizationResponseServer = new AuthorizationResponseServer(responseUrl);
            authorizationResponseServer.Start();

            var authorizationUrl = client.RequestUserAuthorization(state);
            TriggerActionHandler(AuthorizationRequired, authorizationUrl);

            string authorizationCode = TryGetAuthorizationCode(authorizationResponseServer);
            authorizationResponseServer.Stop();
            
            if (!string.IsNullOrEmpty(authorizationCode))
            {
                state = client.ProcessUserAuthorization(authorizationCode, state);
                AuthorizationStorage.SaveRefreshToken(state.RefreshToken, _refreshTokenFilePath);
            }

            return state;
        }

        private string TryGetAuthorizationCode(AuthorizationResponseServer server)
        {
            try
            {
                return server.GetAuthorizationCode();
            }
            catch (AuthorizationCanceledException)
            {
                TriggerActionHandler(AuthorizationCanceled);
            }
            catch (NotSupportedException)
            {
                TriggerActionHandler(AuthorizationNotSupported);
            }

            return null;
        }

        private IAuthorizationState RefreshCredentials(NativeApplicationClient client, string refreshToken)
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

        private Uri FormatResponseUrl()
        {
            var applicationNAme = Assembly.GetEntryAssembly().GetName().Name;
            var port = GetRandomUnusedPort();
            string url = string.Format(AuthorizationResponseUrlFormat, port, applicationNAme);
            return new Uri(url);
        }

        private int GetRandomUnusedPort()
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

        public void Logout()
        {
            AuthorizationStorage.DeleteRefreshToken(_refreshTokenFilePath);
        }
    }
}