using System;
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

        private static void TriggerAuthorizationRequired(Uri authorizationUrl)
        {
            var handler = AuthorizationRequired;
            if (handler != null) 
                handler(authorizationUrl);
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

             IAuthorizationState state = new AuthorizationState(scopes);
             var responseUrl = FormatResponseUrl(client, state);
             state.Callback = responseUrl;

             var authorizationResponseServer = new AuthorizationResponseServer(responseUrl);
             authorizationResponseServer.Start();

             var authorizationUrl = client.RequestUserAuthorization(state);
             TriggerAuthorizationRequired(authorizationUrl);

             string authorizationCode = authorizationResponseServer.GetAuthorizationCode();
             authorizationResponseServer.Stop();

             return client.ProcessUserAuthorization(authorizationCode, state);
         }

        private static Uri FormatResponseUrl(NativeApplicationClient client, IAuthorizationState state)
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