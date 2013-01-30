using System.Threading;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Tasks
{
    public class ShowTasks : ICommand
    {
        private readonly string _tasksListsId;
        private readonly EventAggregator _eventAggregator;
        private readonly TasksService _tasksService;
        private readonly IBusyIndicator _busyIndicator;

        public ShowTasks(
            string tasksListsId,
            EventAggregator eventAggregator,
            TasksService tasksService,
            IBusyIndicator busyIndicator
            )
        {
            _tasksListsId = tasksListsId;
            _eventAggregator = eventAggregator;
            _tasksService = tasksService;
            _busyIndicator = busyIndicator;
        }

        public void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                var tasks = _tasksService.Tasks.List(_tasksListsId).Fetch();
                _eventAggregator.Publish(new TasksUpdated(tasks.Items));
            }
        }
    }
}