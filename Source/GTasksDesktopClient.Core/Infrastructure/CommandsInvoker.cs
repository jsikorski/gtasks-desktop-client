using System;
using System.Threading;
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
                                                     ExceptionsHandler.Handle(exception, command);
                                                 }
                                             });
        }

        
    }
}