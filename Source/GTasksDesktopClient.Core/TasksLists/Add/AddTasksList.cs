using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Synchronization;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists.Add
{
    public class AddTasksList : ICommand
    {
        private readonly string _listTitle;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;
        private readonly CurrentDataContext _currentDataContext;
        private readonly SynchronizationContext _synchronizationContext;

        public AddTasksList(
            string listTitle,
            TasksService tasksService,
            IBusyIndicator busyIndicator,
            CurrentDataContext currentDataContext,
            SynchronizationContext synchronizationContext)
        {
            _listTitle = listTitle;
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

                AddList();
                UpdateLists();

                _synchronizationContext.Unlock();
            }
        }

        private void AddList()
        {
            var tasksList = new TaskList { Title = _listTitle };
            tasksList = _tasksService.Tasklists.Insert(tasksList).Fetch();
            _tasksService.Tasks.Insert(new Task(), tasksList.Id).Fetch();
        }

        private void UpdateLists()
        {
            var tasksLists = _tasksService.Tasklists.List().Fetch();
            _currentDataContext.TasksLists = tasksLists.Items;
            _synchronizationContext.LastTasksListsETag = tasksLists.ETag;
        }
    }
}