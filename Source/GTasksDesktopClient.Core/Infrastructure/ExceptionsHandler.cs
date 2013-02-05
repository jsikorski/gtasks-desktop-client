using System;
using GTasksDesktopClient.Core.Utils;

namespace GTasksDesktopClient.Core.Infrastructure
{
    public static class ExceptionsHandler
    {
        public static void Handle(Exception exception, IExecutable executable)
        {
            if (executable.HandlesException(exception))
            {
                ((dynamic)executable).HandleException((dynamic)exception);
            }
            else
            {
                HandleUnknownExceptions();
            }
        }

        private static void HandleUnknownExceptions()
        {
            MessageBoxService.ShowError("Wystąpił nieznany błąd.");
        }
    }
}