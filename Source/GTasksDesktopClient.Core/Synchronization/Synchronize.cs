using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;
using GTasksDesktopClient.Core.Shell;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class Synchronize : IBackgroundTask
    {
        private readonly DataContext _dataContext;
        private readonly TasksService _tasksService;
        private readonly ISyncStateIndicator _syncStateIndicator;

        public Synchronize(
            DataContext dataContext,
            TasksService tasksService, 
            ISyncStateIndicator syncStateIndicator)
        {
            _dataContext = dataContext;
            _tasksService = tasksService;
            _syncStateIndicator = syncStateIndicator;
        }

        public void Execute()
        {
            using (var dataAccess = _dataContext.GetReadWriteAccess())
            {
                using (new SynchronizationScope(_syncStateIndicator))
                {
                    SynchronizeLists(dataAccess);
                    SynchronizeTasks(dataAccess);
                }
            }
        }

        private void SynchronizeLists(DataContext.ReadWriteAccess dataAccess)
        {
            dataAccess.UpdateTasksLists(_tasksService);
            UpdateLastLoadedTasksListId(dataAccess);
        }

        private void UpdateLastLoadedTasksListId(DataContext.ReadWriteAccess dataAccess)
        {
            dataAccess.ValidateLastLoadedTasksListsId();
        }

        private void SynchronizeTasks(DataContext.ReadWriteAccess dataAccess)
        {
            if (!IsAnyTasksListSelected(dataAccess))
                return;

            dataAccess.UpdateTasks(_tasksService, dataAccess.LastLoadedTasksListId);
        }

        private bool IsAnyTasksListSelected(DataContext.ReadWriteAccess dataAccess)
        {
            return dataAccess.LastLoadedTasksListId != null;
        }
    }
}