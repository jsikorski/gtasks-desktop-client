using System.IO;
using System.Reflection;

namespace GApiHelpers.Authorization
{
    internal static class ResourcesHelper
    {
        private const string ResourcesPathFormat = "{0}.Resources.{1}";

         public static string GetAsString(string relativeResourcePath)
         {
             var resourceStream = GetResourceStream(relativeResourcePath);
             using (var streamReader = new StreamReader(resourceStream))
             {
                 return streamReader.ReadToEnd();
             }
         }

        private static Stream GetResourceStream(string relativeResourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = ResolveResourcePath(relativeResourcePath, assembly);
            var resourceStream = assembly.GetManifestResourceStream(resourcePath);
            return resourceStream;
        }

        private static string ResolveResourcePath(string relativeResourcePath, Assembly assembly)
        {
            return string.Format(ResourcesPathFormat, assembly.GetName().Name, relativeResourcePath);
        }
    }
}