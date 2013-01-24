using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Lists
{
    public class GetAllLists : ICommand
    {
        private readonly TasksService _tasksService;
        private readonly IBusyScope _busyScope;

        public GetAllLists(TasksService tasksService, IBusyScope busyScope)
        {
            _tasksService = tasksService;
            _busyScope = busyScope;
        }

        public void Execute()
        {
            using (var busyScopeContext = new BusyScopeContext(_busyScope))
            {
                var lists = _tasksService.Tasklists.List().Fetch();                
            }
        }
    }
}