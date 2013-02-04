using GTasksDesktopClient.Core.Api;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Tasks
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
                using (var dataContext = _dataAccessController.GetContext())
                {
                    if (_tasksListsId == dataContext.LastLoadedTasksListId)
                        return;

                    dataContext.UpdateTasks(_tasksService, _tasksListsId);
                }
            }
        }
    }
}