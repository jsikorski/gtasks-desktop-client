using System;
using System.IO;
using System.Net;

namespace GApiHelpers.Authorization
{
    public class AuthorizationResponseServer
    {
        private readonly Uri _responseUrl;
        private HttpListener _webServer;

        public AuthorizationResponseServer(Uri responseUrl)
        {
            _responseUrl = responseUrl;
        }

        public void Start()
        {
            _webServer = new HttpListener();
            _webServer.Prefixes.Add(_responseUrl.ToString());
            _webServer.Start();
        }

        public void Stop()
        {
            _webServer.Stop();
        }

        public string GetAuthorizationCode()
        {
            var context = _webServer.GetContext();
            return HandleContext(context);
        }

        private string HandleContext(HttpListenerContext context)
        {
            WriteResponse(context);

            string code = context.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))
                return code;

            string error = context.Request.QueryString["error"];
            if (!string.IsNullOrEmpty(error))
                throw new AuthorizationCanceledException();

            throw new NotSupportedException("Authorization method is not supported by client.");
        }

        private static void WriteResponse(HttpListenerContext context)
        {
            using (var writer = new StreamWriter(context.Response.OutputStream))
            {
                const string response = 
                    "<script type='text/javascript'>currentWindow = window.open(window.location, '_self'); currentWindow.close();</script>";
                writer.Write(response);
                writer.Flush();
            }
        }
    }
}