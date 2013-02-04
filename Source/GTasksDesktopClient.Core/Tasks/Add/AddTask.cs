using GTasksDesktopClient.Core.Api;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Tasks.Add
{
    public class AddTask : ApiCommand
    {
        private readonly Task _task;
        private readonly TasksService _tasksService;
        private readonly DataAccessController _dataAccessController;
        private readonly IBusyIndicator _busyIndicator;

        public AddTask(
            Task task, 
            TasksService tasksService, 
            DataAccessController dataAccessController, 
            IBusyIndicator busyIndicator)
        {
            _task = task;
            _tasksService = tasksService;
            _dataAccessController = dataAccessController;
            _busyIndicator = busyIndicator;
        }

        public override void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                using (var dataAccess = _dataAccessController.GetReadWriteAccess())
                {
                    _tasksService.Tasks.Insert(_task, dataAccess.LastLoadedTasksListId).Fetch();
                    dataAccess.UpdateTasks(_tasksService, dataAccess.LastLoadedTasksListId);
                }
            }
        }
    }
}