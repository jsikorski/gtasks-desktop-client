using GTasksDesktopClient.Core.Api;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Tasks.Load
{
    public class LoadTasks : ApiCommand
    {
        private readonly string _tasksListsId;
        private readonly DataContext _dataContext;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;

        public LoadTasks(
            string tasksListsId,
            DataContext dataContext,
            TasksService tasksService,
            IBusyIndicator busyIndicator)
        {
            _tasksListsId = tasksListsId;
            _dataContext = dataContext;
            _tasksService = tasksService;
            _busyIndicator = busyIndicator;
        }

        public override void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                using (var dataAccess = _dataContext.GetReadWriteAccess())
                {
                    if (_tasksListsId == dataAccess.LastLoadedTasksListId)
                        return;

                    dataAccess.UpdateTasks(_tasksService, _tasksListsId);
                }
            }
        }
    }
}