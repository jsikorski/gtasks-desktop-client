using GTasksDesktopClient.Core.Api;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists.Add
{
    public class AddTasksList : ApiCommand
    {
        private readonly string _listTitle;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;
        private readonly DataAccessController _dataAccessController;

        public AddTasksList(
            string listTitle,
            TasksService tasksService,
            IBusyIndicator busyIndicator,
            DataAccessController dataAccessController)
        {
            _listTitle = listTitle;
            _tasksService = tasksService;
            _busyIndicator = busyIndicator;
            _dataAccessController = dataAccessController;
        }

        public override void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                using (var dataContext = _dataAccessController.GetContext())
                {
                    AddList();
                    dataContext.UpdateTasksLists(_tasksService);
                }
            }
        }

        private void AddList()
        {
            var tasksList = new TaskList { Title = _listTitle };
            tasksList = _tasksService.Tasklists.Insert(tasksList).Fetch();
            _tasksService.Tasks.Insert(new Task(), tasksList.Id).Fetch();
        }
    }
}