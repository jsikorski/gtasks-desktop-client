using System.Collections.Generic;
using Caliburn.Micro;
using GTasksDesktopClient.Core.TasksLists;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core
{
    public class CurrentContext
    {
        private readonly IEventAggregator _eventAggregator;

        public string SelectedTasksListId { get; set; }

        private string _lastTasksListsETag;
        public IEnumerable<TaskList> TasksLists { get; private set; }

        public void UpdateTasksLists(TaskLists taskLists)
        {
            if (_lastTasksListsETag == taskLists.ETag)
                return;

            _lastTasksListsETag = taskLists.ETag;
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