using System.Collections.Generic;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Lists;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core
{
    public class CurrentContext
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

        public CurrentContext(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _tasksLists = new List<TaskList>();
        }
    }
}