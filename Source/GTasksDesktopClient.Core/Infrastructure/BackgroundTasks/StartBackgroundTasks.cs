using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Autofac;

namespace GTasksDesktopClient.Core.Infrastructure.BackgroundTasks
{
    public class StartBackgroundTasks : IStartable
    {
        private readonly IContainer _container;
        private readonly BackgroundTasksContext _backgroundTasksContext;

        public StartBackgroundTasks(
            IContainer container, 
            BackgroundTasksContext backgroundTasksContext)
        {
            _container = container;
            _backgroundTasksContext = backgroundTasksContext;
        }

        public void Start()
        {
            using (var timerUsageScope = _backgroundTasksContext.GetTimeUsageScope())
            {
                timerUsageScope.Timer.Elapsed += ExecuteTasks;
                timerUsageScope.Timer.AutoReset = false;
                timerUsageScope.Timer.Start();
            }
        }

        private void ExecuteTasks(object state, ElapsedEventArgs elapsedEventArgs)
        {
            using (var timerUsageScope = _backgroundTasksContext.GetTimeUsageScope())
            {
                var backgroundTasks = _container.Resolve<IEnumerable<IBackgroundTask>>();
                backgroundTasks.ToList().ForEach(BackgroundTasksInvoker.ExecuteTask);

                timerUsageScope.Timer.Interval = Properties.Settings.Default.BackgroundTasksFrequency;
                timerUsageScope.Timer.Start();
            }
        }
    }
}