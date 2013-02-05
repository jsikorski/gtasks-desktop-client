﻿using System;
using System.Threading.Tasks;

namespace GTasksDesktopClient.Core.Infrastructure.BackgroundTasks
{
    public class BackgroundTasksInvoker
    {
        public static void ExecuteTask(IBackgroundTask backgroundTask)
        {
            try
            {
                backgroundTask.Execute();
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception exception)
            {
                ExceptionsHandler.Handle(exception, backgroundTask);
            }
        }
    }
}