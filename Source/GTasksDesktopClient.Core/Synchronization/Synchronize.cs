using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class Synchronize : IBackgroundTask
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly CurrentContext _currentContext;
        private readonly TasksService _tasksService;
        private readonly ISyncStateIndicator _syncStateIndicator;

        public Synchronize(
            SynchronizationContext synchronizationContext,
            CurrentContext currentContext,
            TasksService tasksService, 
            ISyncStateIndicator syncStateIndicator)
        {
            _synchronizationContext = synchronizationContext;
            _currentContext = currentContext;
            _tasksService = tasksService;
            _syncStateIndicator = syncStateIndicator;
        }

        public void Execute()
        {
            _synchronizationContext.Lock();

            using (new SynchronizationScope(_syncStateIndicator))
            {
                var lists = _tasksService.Tasklists.List().Fetch();

                if (_synchronizationContext.LastTasksListsETag != lists.ETag)
                {
                    _currentContext.TasksLists = lists.Items;
                    _synchronizationContext.LastTasksListsETag = lists.ETag;
                }
            }
        
            _synchronizationContext.Unlock();
        }
    }
}