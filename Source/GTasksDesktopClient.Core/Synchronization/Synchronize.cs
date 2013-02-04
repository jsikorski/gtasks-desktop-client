using GTasksDesktopClient.Core.DataAccess;
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
            using (var dataAccess = _dataAccessController.GetReadWriteAccess())
            {
                using (new SynchronizationScope(_syncStateIndicator))
                {
                    SynchronizeLists(dataAccess);
                    SynchronizeTasks(dataAccess);
                }
            }
        }

        private void SynchronizeLists(DataAccessController.ReadWriteAccess dataAccess)
        {
            dataAccess.UpdateTasksLists(_tasksService);
            UpdateLastLoadedTasksListId(dataAccess);
        }

        private void UpdateLastLoadedTasksListId(DataAccessController.ReadWriteAccess dataAccess)
        {
            dataAccess.ValidateLastLoadedTasksListsId();
        }

        private void SynchronizeTasks(DataAccessController.ReadWriteAccess dataAccess)
        {
            if (!IsAnyTasksListSelected(dataAccess))
                return;

            dataAccess.UpdateTasks(_tasksService, dataAccess.LastLoadedTasksListId);
        }

        private bool IsAnyTasksListSelected(DataAccessController.ReadWriteAccess dataAccess)
        {
            return dataAccess.LastLoadedTasksListId != null;
        }
    }
}