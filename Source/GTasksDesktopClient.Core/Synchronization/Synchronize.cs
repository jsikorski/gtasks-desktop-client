using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class Synchronize : IBackgroundTask
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly TasksService _tasksService;
        private readonly IBusyScope _busyScope;

        public Synchronize(
            SynchronizationContext synchronizationContext, 
            TasksService tasksService, 
            IBusyScope busyScope)
        {
            _synchronizationContext = synchronizationContext;
            _tasksService = tasksService;
            _busyScope = busyScope;
        }

        public void Execute()
        {
            using (var busyScopeContext = new BusyScopeContext(_busyScope))
            {
                var lists = _tasksService.Tasklists.List().Fetch();
                _synchronizationContext.TasksLists = lists.Items;
            }   
        }
    }
}