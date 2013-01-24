using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Lists
{
    public class GetAllLists : ICommand
    {
        private readonly TasksService _tasksService;

        public GetAllLists(TasksService tasksService)
        {
            _tasksService = tasksService;
        }

        public void Execute()
        {
            var lists = _tasksService.Tasklists.List().Fetch();
        }
    }
}