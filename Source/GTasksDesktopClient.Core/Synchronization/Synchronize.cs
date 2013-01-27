using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class Synchronize : IBackgroundTask
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly TasksService _tasksService;

        public Synchronize(
            SynchronizationContext synchronizationContext,
            TasksService tasksService)
        {
            _synchronizationContext = synchronizationContext;
            _tasksService = tasksService;
        }

        public void Execute()
        {
            var lists = _tasksService.Tasklists.List().Fetch();
            _synchronizationContext.TasksLists = lists.Items;
        }
    }
}