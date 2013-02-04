using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists.Edit
{
    public class EditTasksList : ICommand
    {
        private readonly TaskList _tasksList;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;
        private readonly DataAccessController _dataAccessController;

        public EditTasksList(
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

        public void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                using (var dataContext = _dataAccessController.GetContext())
                {
                    UpdateList();
                    dataContext.UpdateTasksLists(_tasksService);
                }
            }
        }

        private void UpdateList()
        {
            _tasksService.Tasklists.Update(_tasksList, _tasksList.Id).Fetch();
        }
    }
}