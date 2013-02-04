using GTasksDesktopClient.Core.Api;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists.Add
{
    public class AddTasksList : ApiCommand
    {
        private readonly TaskList _tasksList;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;
        private readonly DataAccessController _dataAccessController;

        public AddTasksList(
            TaskList tasksList,
            TasksService tasksService,
            IBusyIndicator busyIndicator,
            DataAccessController dataAccessController)
        {
            _tasksList = tasksList;
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
                    _tasksService.Tasklists.Insert(_tasksList).Fetch();
                    dataAccess.UpdateTasksLists(_tasksService);
                }
            }
        }
    }
}