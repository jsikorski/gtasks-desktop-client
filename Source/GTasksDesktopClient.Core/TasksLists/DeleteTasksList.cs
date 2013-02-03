using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Synchronization;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.TasksLists
{
    public class DeleteTasksList : ICommand
    {
        private readonly string _tasksListId;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;
        private readonly SynchronizationContext _synchronizationContext;
        private readonly CurrentDataContext _currentDataContext;

        public DeleteTasksList(
            string tasksListId, 
            TasksService tasksService, 
            IBusyIndicator busyIndicator, 
            SynchronizationContext synchronizationContext,
            CurrentDataContext currentDataContext)
        {
            _tasksListId = tasksListId;
            _tasksService = tasksService;
            _busyIndicator = busyIndicator;
            _synchronizationContext = synchronizationContext;
            _currentDataContext = currentDataContext;
        }

        public void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                _synchronizationContext.Lock();

                DeleteList();
                UpdateLists();

                _synchronizationContext.Unlock();
            }
        }

        private void DeleteList()
        {
            _tasksService.Tasklists.Delete(_tasksListId).Fetch();
        }

        private void UpdateLists()
        {
            var tasksLists = _tasksService.Tasklists.List().Fetch();
            _currentDataContext.TasksLists = tasksLists.Items;
            _synchronizationContext.LastTasksListsETag = tasksLists.ETag;
        }
    }
}