using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Synchronization;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Tasks
{
    public class LoadTasks : ICommand
    {
        private readonly string _tasksListsId;
        private readonly TasksService _tasksService;
        private readonly CurrentDataContext _currentDataContext;
        private readonly IBusyIndicator _busyIndicator;
        private readonly SynchronizationContext _synchronizationContext;

        public LoadTasks(
            string tasksListsId,
            TasksService tasksService,
            CurrentDataContext currentDataContext,
            IBusyIndicator busyIndicator, 
            SynchronizationContext synchronizationContext)
        {
            _tasksListsId = tasksListsId;
            _tasksService = tasksService;
            _currentDataContext = currentDataContext;
            _busyIndicator = busyIndicator;
            _synchronizationContext = synchronizationContext;
        }

        public void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                _synchronizationContext.Lock();

                if (_tasksListsId == _currentDataContext.LastLoadedTasksListId)
                {
                    _synchronizationContext.Unlock();
                    return;
                }
                    
                _currentDataContext.LastLoadedTasksListId = _tasksListsId;

                var tasks = _tasksService.Tasks.List(_tasksListsId).Fetch();
                _currentDataContext.Tasks = tasks.Items;

                _synchronizationContext.Unlock();
            }
        }
    }
}