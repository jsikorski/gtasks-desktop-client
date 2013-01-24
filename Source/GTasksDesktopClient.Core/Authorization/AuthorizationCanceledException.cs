using System;

namespace GTasksDesktopClient.Core.Authorization
{
    public class AuthorizationCanceledException : Exception
    {
        public AuthorizationCanceledException() : base("Operation was canceled by user.")
        {
        }
    }
}