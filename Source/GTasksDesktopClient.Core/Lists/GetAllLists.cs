using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Lists
{
    public class GetAllLists : ICommand
    {
        private readonly TasksService _tasksService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IBusyScope _busyScope;

        public GetAllLists(
            TasksService tasksService,
            IEventAggregator eventAggregator,
            IBusyScope busyScope)
        {
            _tasksService = tasksService;
            _eventAggregator = eventAggregator;
            _busyScope = busyScope;
        }

        public void Execute()
        {
            using (var busyScopeContext = new BusyScopeContext(_busyScope))
            {
                var lists = _tasksService.Tasklists.List().Fetch();
                _eventAggregator.Publish(new ListsFetched {TasksLists = lists.Items});
            }
        }
    }
}