using GTasksDesktopClient.Core.Api;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists.Delete
{
    public class DeleteTasksList : ApiCommand
    {
        private readonly TaskList _taskList;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;
        private readonly DataAccessController _dataAccessController;

        public DeleteTasksList(
            TaskList taskList, 
            TasksService tasksService, 
            IBusyIndicator busyIndicator, 
            DataAccessController dataAccessController)
        {
            _taskList = taskList;
            _tasksService = tasksService;
            _busyIndicator = busyIndicator;
            _dataAccessController = dataAccessController;
        }

        public override void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                using (var dataAccess = _dataAccessController.GetReadWriteAccess())
                {
                    _tasksService.Tasklists.Delete(_taskList.Id).Fetch();
                    dataAccess.UpdateTasksLists(_tasksService);
                }
            }
        }
    }
}