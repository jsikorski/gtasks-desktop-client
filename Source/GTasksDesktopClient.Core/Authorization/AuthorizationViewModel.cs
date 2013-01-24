using System;
using System.Windows;
using System.Windows.Controls;

namespace GTasksDesktopClient.Core.Authorization
{
    public class AuthorizationViewModel
    {
        public Uri AuthorizationUrl { get; private set; }

        public AuthorizationViewModel(Uri authorizationUrl)
        {
            AuthorizationUrl = authorizationUrl;
        }
    }
}