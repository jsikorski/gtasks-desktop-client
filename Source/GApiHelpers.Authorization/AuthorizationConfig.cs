using System.Collections.Generic;

namespace GApiHelpers.Authorization
{
    public class AuthorizationConfig
    {
        public string ClientIdentifier { get; set; }
        public string ClientSecret { get; set; }
        public IEnumerable<string> Scopes { get; set; }
        public string RefreshTokenFilePath { get; set; }
    }
}