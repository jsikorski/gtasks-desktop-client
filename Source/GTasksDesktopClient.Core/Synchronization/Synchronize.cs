using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class Synchronize : IBackgroundTask
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly CurrentContext _currentContext;
        private readonly TasksService _tasksService;

        public Synchronize(
            SynchronizationContext synchronizationContext,
            CurrentContext currentContext,
            TasksService tasksService)
        {
            _synchronizationContext = synchronizationContext;
            _currentContext = currentContext;
            _tasksService = tasksService;
        }

        public void Execute()
        {
            _synchronizationContext.Lock();

            var lists = _tasksService.Tasklists.List().Fetch();
            _currentContext.TasksLists = lists.Items;
        
            _synchronizationContext.Unlock();
        }
    }
}