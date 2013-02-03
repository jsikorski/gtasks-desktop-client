using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace GTasksDesktopClient.Core.Infrastructure.BackgroundTasks
{
    public class BackgroundTasksManager
    {
        private readonly IContainer _container;

        public BackgroundTasksManager(IContainer container)
        {
            _container = container;
        }

        public void StartAll()
        {
            var startables = _container.Resolve<IEnumerable<IStartable>>();
            startables.ToList().ForEach(startable => startable.Start());
        }

        public void StopAll()
        {
            var stopables = _container.Resolve<IEnumerable<IStopable>>();
            stopables.ToList().ForEach(stopable => stopable.Stop());
        }
    }
}