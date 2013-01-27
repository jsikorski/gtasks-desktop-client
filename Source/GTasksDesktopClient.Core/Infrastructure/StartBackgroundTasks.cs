using System;
using System.Collections.Generic;
using System.Threading;
using Autofac;
using System.Linq;

namespace GTasksDesktopClient.Core.Infrastructure
{
    public class StartBackgroundTasks : IStartable
    {
        private readonly IContainer _container;
        private Timer _backgroundTasksTimer;

        public StartBackgroundTasks(IContainer container)
        {
            _container = container;
        }

        public void Execute()
        {
            _backgroundTasksTimer = new Timer(ExecuteTasks, null, 0, Timeout.Infinite);
        }

        private void ExecuteTasks(object state)
        {
            var backgroundTasks = _container.Resolve<IEnumerable<IBackgroundTask>>();
            backgroundTasks.ToList().ForEach(task => task.Execute());
            
            _backgroundTasksTimer.Change(1000, Timeout.Infinite);
        }
    }
}