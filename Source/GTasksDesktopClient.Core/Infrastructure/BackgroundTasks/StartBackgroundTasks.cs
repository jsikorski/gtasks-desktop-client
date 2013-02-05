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
            _backgroundTasksContext.Timer.Elapsed += ExecuteTasks;
            _backgroundTasksContext.Timer.AutoReset = false;
            _backgroundTasksContext.Timer.Start();
        }

        private void ExecuteTasks(object state, ElapsedEventArgs elapsedEventArgs)
        {
            var backgroundTasks = _container.Resolve<IEnumerable<IBackgroundTask>>();
            backgroundTasks.ToList().ForEach(BackgroundTasksInvoker.ExecuteTask);

            _backgroundTasksContext.Timer.Interval = Properties.Settings.Default.BackgroundTasksFrequency;
            _backgroundTasksContext.Timer.Start();
        }
    }
}