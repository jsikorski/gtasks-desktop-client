namespace GTasksDesktopClient.Core.Infrastructure.BackgroundTasks
{
    public class StopBackgroundTasks : IStopable
    {
        private readonly BackgroundTasksContext _backgroundTasksContext;

        public StopBackgroundTasks(BackgroundTasksContext backgroundTasksContext)
        {
            _backgroundTasksContext = backgroundTasksContext;
        }

        public void Stop()
        {
            _backgroundTasksContext.Timer.Stop();
        }
    }
}