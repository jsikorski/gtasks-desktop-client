using System;

namespace GApiHelpers.Authorization
{
    public class AuthorizationCanceledException : Exception
    {
        public AuthorizationCanceledException() : base("Operation was canceled by user.")
        {
        }
    }
}