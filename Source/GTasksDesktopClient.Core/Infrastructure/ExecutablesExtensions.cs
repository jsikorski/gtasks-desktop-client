using System;
using System.Linq;

namespace GTasksDesktopClient.Core.Infrastructure
{
    public static class ExecutablesExtensions
    {
        public static bool HandlesException(this IExecutable executable, Exception exception)
        {
            var handlerType = typeof (IHandleException<>).MakeGenericType(exception.GetType());
            return executable.GetType().GetInterfaces().Any(i => i == handlerType);
        }
    }
}