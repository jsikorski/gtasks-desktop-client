using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;
using GTasksDesktopClient.Core.Shell;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class Synchronize : IBackgroundTask
    {
        private readonly DataAccessController _dataAccessController;
        private readonly TasksService _tasksService;
        private readonly ISyncStateIndicator _syncStateIndicator;

        public Synchronize(
            DataAccessController dataAccessController,
            TasksService tasksService, 
            ISyncStateIndicator syncStateIndicator)
        {
            _dataAccessController = dataAccessController;
            _tasksService = tasksService;
            _syncStateIndicator = syncStateIndicator;
        }

        public void Execute()
        {
            using (var dataContext = _dataAccessController.GetContext())
            {
                using (new SynchronizationScope(_syncStateIndicator))
                {
                    SynchronizeLists(dataContext);
                    SynchronizeTasks(dataContext);
                }
            }
        }

        private void SynchronizeLists(DataAccessController.DataContext dataContext)
        {
            dataContext.UpdateTasksLists(_tasksService);
            UpdateLastLoadedTasksListId(dataContext);
        }

        private void UpdateLastLoadedTasksListId(DataAccessController.DataContext dataContext)
        {
            dataContext.ValidateLastLoadedTasksListsId();
        }

        private void SynchronizeTasks(DataAccessController.DataContext dataContext)
        {
            if (!IsAnyTasksListSelected(dataContext))
                return;

            dataContext.UpdateTasks(_tasksService, dataContext.LastLoadedTasksListId);
        }

        private bool IsAnyTasksListSelected(DataAccessController.DataContext dataContext)
        {
            return dataContext.LastLoadedTasksListId != null;
        }
    }
}