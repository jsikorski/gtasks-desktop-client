using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace GTasksDesktopClient.Core.Infrastructure.BackgroundTasks
{
    public class BackgroundTasksContext : IDisposable
    {
        private Semaphore Semaphore { get; set; }
        private Timer Timer { get; set; }

        public BackgroundTasksContext()
        {
            Semaphore = new Semaphore(1, 1);
            Timer = new Timer(1);
        }

        public TimerUsageScope GetTimeUsageScope()
        {
            return new TimerUsageScope(this);
        }

        public void Dispose()
        {
            Timer.Dispose();
        }

        public class TimerUsageScope : IDisposable
        {
            private readonly BackgroundTasksContext _backgroundTasksContext;

            public Timer Timer
            {
                get { return _backgroundTasksContext.Timer; }
            }

            public TimerUsageScope(BackgroundTasksContext backgroundTasksContext)
            {
                _backgroundTasksContext = backgroundTasksContext;

                _backgroundTasksContext.Semaphore.WaitOne();
            }

            public void Dispose()
            {
                _backgroundTasksContext.Semaphore.Release();
            }
        }
    }
}