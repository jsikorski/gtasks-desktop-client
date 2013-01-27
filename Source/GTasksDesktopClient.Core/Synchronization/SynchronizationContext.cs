using System.Collections.Generic;
using Caliburn.Micro;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class SynchronizationContext
    {
        private readonly IEventAggregator _eventAggregator;

        private IEnumerable<TaskList> _tasksLists;
        public IEnumerable<TaskList> TasksLists
        {
            get { return _tasksLists; }
            set
            {
                _tasksLists = value;
                _eventAggregator.Publish(new ListsUpdated(value));
            }
        }

        public SynchronizationContext(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _tasksLists = new List<TaskList>();
        }
    }
}