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
        private readonly DataContext _dataContext;

        public DeleteTasksList(
            TaskList taskList, 
            TasksService tasksService, 
            IBusyIndicator busyIndicator, 
            DataContext dataContext)
        {
            _taskList = taskList;
            _tasksService = tasksService;
            _busyIndicator = busyIndicator;
            _dataContext = dataContext;
        }

        public override void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                using (var dataAccess = _dataContext.GetReadWriteAccess())
                {
                    _tasksService.Tasklists.Delete(_taskList.Id).Fetch();
                    dataAccess.UpdateTasksLists(_tasksService);
                }
            }
        }
    }
}