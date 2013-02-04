using GTasksDesktopClient.Core.Api;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Tasks.Edit
{
    public class EditTask : ApiCommand
    {
        private readonly Task _task;
        private readonly DataAccessController _dataAccessController;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;

        public EditTask(
            Task task,
            DataAccessController dataAccessController, 
            TasksService tasksService, 
            IBusyIndicator busyIndicator)
        {
            _task = task;
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
                    _tasksService.Tasks.Update(_task, dataAccess.LastLoadedTasksListId, _task.Id).Fetch();
                    dataAccess.UpdateTasks(_tasksService, dataAccess.LastLoadedTasksListId);
                }
            }
        }
    }
}