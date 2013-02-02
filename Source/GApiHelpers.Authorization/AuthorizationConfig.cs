using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GApiHelpers.Authorization
{
    public class AuthorizationConfig
    {
        public string ClientIdentifier { get; set; }
        public string ClientSecret { get; set; }
        public IEnumerable<string> Scopes { get; set; }
        public string RefreshTokenFilePath { get; set; }
    
        public void Validate()
        {
            if (Scopes == null || !Scopes.Any())
                throw new ArgumentException("Scopes cannot be null or empty");

            ValidateStringProperties();
        }

        private bool IsString(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof (string);
        }

        private void ValidateStringProperties()
        {
            GetType().GetProperties().Where(IsString)
                .ToList().ForEach(ValidateStringProperty);
        }

        private void ValidateStringProperty(PropertyInfo propertyInfo)
        {
            var propertyValue = (string)propertyInfo.GetValue(this, null);
            if (string.IsNullOrEmpty(propertyValue))
                throw new ArgumentNullException(propertyInfo.Name, "Value is required");
        }
    }
}