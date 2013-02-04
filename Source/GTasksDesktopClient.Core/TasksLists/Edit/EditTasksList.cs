using GTasksDesktopClient.Core.Api;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists.Edit
{
    public class EditTasksList : ApiCommand
    {
        private readonly TaskList _tasksList;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;
        private readonly DataContext _dataContext;

        public EditTasksList(
            TaskList tasksList, 
            TasksService tasksService, 
            IBusyIndicator busyIndicator, 
            DataContext dataContext)
        {
            _tasksList = tasksList;
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
                    UpdateList();
                    dataAccess.UpdateTasksLists(_tasksService);
                }
            }
        }

        private void UpdateList()
        {
            _tasksService.Tasklists.Update(_tasksList, _tasksList.Id).Fetch();
        }
    }
}