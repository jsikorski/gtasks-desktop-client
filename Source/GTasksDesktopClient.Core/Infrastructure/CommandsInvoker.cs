using System;
using System.Threading;
using System.Linq;
using GTasksDesktopClient.Core.Utils;

namespace GTasksDesktopClient.Core.Infrastructure
{
    public class CommandsInvoker
    {
        public static void ExecuteCommand(ICommand command)
        {
            ThreadPool.QueueUserWorkItem(state =>
                                             {
                                                 try
                                                 {
                                                     command.Execute();
                                                 }
                                                 catch (Exception exception)
                                                 {
                                                     HandleException(exception, command);
                                                 }
                                             });
        }

        private static void HandleException(Exception exception, ICommand command)
        {
            if (command.HandlesException(exception))
            {
                ((dynamic) command).HandleException((dynamic)exception);
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