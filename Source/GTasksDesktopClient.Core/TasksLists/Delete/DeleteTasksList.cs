using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.TasksLists.Delete
{
    public class DeleteTasksList : ICommand
    {
        private readonly string _tasksListId;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;
        private readonly DataAccessController _dataAccessController;

        public DeleteTasksList(
            string tasksListId, 
            TasksService tasksService, 
            IBusyIndicator busyIndicator, 
            DataAccessController dataAccessController)
        {
            _tasksListId = tasksListId;
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
                    DeleteList();
                    dataContext.UpdateTasksLists(_tasksService);
                }
            }
        }

        private void DeleteList()
        {
            _tasksService.Tasklists.Delete(_tasksListId).Fetch();
        }
    }
}