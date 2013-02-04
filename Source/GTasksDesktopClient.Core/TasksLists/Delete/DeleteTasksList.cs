using GTasksDesktopClient.Core.Api;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.TasksLists.Delete
{
    public class DeleteTasksList : ApiCommand
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

        public override void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                using (var dataAccess = _dataAccessController.GetReadWriteAccess())
                {
                    DeleteList();
                    dataAccess.UpdateTasksLists(_tasksService);
                }
            }
        }

        private void DeleteList()
        {
            _tasksService.Tasklists.Delete(_tasksListId).Fetch();
        }
    }
}