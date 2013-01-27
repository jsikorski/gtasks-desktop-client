using System.Collections.Generic;
using Autofac;
using System.Linq;

namespace GTasksDesktopClient.Core.Infrastructure
{
    public class StartBackgroundTasks : IStartable
    {
        private readonly IContainer _container;

        public StartBackgroundTasks(IContainer container)
        {
            _container = container;
        }

        public void Execute()
        {
            var backgroundTasks = _container.Resolve<IEnumerable<IBackgroundTask>>();
            backgroundTasks.ToList().ForEach(task => task.Execute());
        }
    }
}