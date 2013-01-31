using System;
using System.Timers;

namespace GTasksDesktopClient.Core.Infrastructure.BackgroundTasks
{
    public class BackgroundTasksContext : IDisposable
    {
        public Timer Timer { get; private set; }

        public BackgroundTasksContext()
        {
            Timer = new Timer(1);
        }

        public void Dispose()
        {
            Timer.Dispose();
        }
    }
}