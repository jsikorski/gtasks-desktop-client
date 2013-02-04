using System;
using System.Linq;

namespace GTasksDesktopClient.Core.Infrastructure
{
    public static class CommandsExtensions
    {
        public static bool HandlesException(this ICommand command, Exception exception)
        {
            var handlerType = typeof (IHandleException<>).MakeGenericType(exception.GetType());
            return command.GetType().GetInterfaces().Any(i => i == handlerType);
        }
    }
}