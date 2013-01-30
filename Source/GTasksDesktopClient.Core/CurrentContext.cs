using System.Collections.Generic;
using Caliburn.Micro;
using GTasksDesktopClient.Core.TasksLists;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core
{
    public class CurrentContext
    {
        private readonly IEventAggregator _eventAggregator;

        private string _lastTasksListETag;
        public IEnumerable<TaskList> TasksLists { get; private set; }

        public void UpdateTasksLists(TaskLists taskLists)
        {
            if (_lastTasksListETag == taskLists.ETag)
                return;

            _lastTasksListETag = taskLists.ETag;
            TasksLists = taskLists.Items;
            _eventAggregator.Publish(new TasksListsUpdated(TasksLists));
        }

        public CurrentContext(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            TasksLists = new List<TaskList>();
        }
    }
}