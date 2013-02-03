using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Synchronization;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists
{
    public class EditTasksList : ICommand
    {
        private readonly TaskList _tasksList;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;
        private readonly CurrentDataContext _currentDataContext;
        private readonly SynchronizationContext _synchronizationContext;

        public EditTasksList(
            TaskList tasksList, 
            TasksService tasksService, 
            IBusyIndicator busyIndicator, 
            CurrentDataContext currentDataContext,
            SynchronizationContext synchronizationContext)
        {
            _tasksList = tasksList;
            _tasksService = tasksService;
            _busyIndicator = busyIndicator;
            _currentDataContext = currentDataContext;
            _synchronizationContext = synchronizationContext;
        }

        public void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                _synchronizationContext.Lock();

                UpdateList();
                UpdateLists();

                _synchronizationContext.Unlock();
            }
        }

        private void UpdateList()
        {
            _tasksService.Tasklists.Update(_tasksList, _tasksList.Id).Fetch();
        }

        private void UpdateLists()
        {
            var tasksLists = _tasksService.Tasklists.List().Fetch();
            _currentDataContext.TasksLists = tasksLists.Items;
            _synchronizationContext.LastTasksListsETag = tasksLists.ETag;
        }
    }
}