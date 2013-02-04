using GTasksDesktopClient.Core.Api;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Tasks.Load
{
    public class LoadTasks : ApiCommand
    {
        private readonly string _tasksListsId;
        private readonly DataAccessController _dataAccessController;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;

        public LoadTasks(
            string tasksListsId,
            DataAccessController dataAccessController,
            TasksService tasksService,
            IBusyIndicator busyIndicator)
        {
            _tasksListsId = tasksListsId;
            _dataAccessController = dataAccessController;
            _tasksService = tasksService;
            _busyIndicator = busyIndicator;
        }

        public override void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                using (var dataAccess = _dataAccessController.GetReadWriteAccess())
                {
                    if (_tasksListsId == dataAccess.LastLoadedTasksListId)
                        return;

                    dataAccess.UpdateTasks(_tasksService, _tasksListsId);
                }
            }
        }
    }
}