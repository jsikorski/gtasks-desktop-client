using System;
using System.Timers;

namespace GTasksDesktopClient.Core.Infrastructure.BackgroundTasks
{
    public class BackgroundTasksContext : IDisposable
    {
        private const int BackgroundTasksInterval = 1000;

        public Timer Timer { get; private set; }

        public BackgroundTasksContext()
        {
            Timer = new Timer(BackgroundTasksInterval);
        }

        public void Dispose()
        {
            Timer.Dispose();
        }
    }
}