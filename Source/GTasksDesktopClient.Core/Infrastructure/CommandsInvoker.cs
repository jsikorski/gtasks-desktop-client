using System;
using System.Threading;

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