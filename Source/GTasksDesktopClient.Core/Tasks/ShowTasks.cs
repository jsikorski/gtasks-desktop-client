using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Shell;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Tasks
{
    public class ShowTasks : ICommand
    {
        private readonly string _tasksListsId;
        private readonly TasksService _tasksService;
        private readonly CurrentDataContext _currentDataContext;
        private readonly IBusyIndicator _busyIndicator;

        public ShowTasks(
            string tasksListsId,
            TasksService tasksService,
            CurrentDataContext currentDataContext,
            IBusyIndicator busyIndicator)
        {
            _tasksListsId = tasksListsId;
            _tasksService = tasksService;
            _currentDataContext = currentDataContext;
            _busyIndicator = busyIndicator;
        }

        public void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                var tasks = _tasksService.Tasks.List(_tasksListsId).Fetch();
                _currentDataContext.Tasks = tasks.Items;
            }
        }
    }
}