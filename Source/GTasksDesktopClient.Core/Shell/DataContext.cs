using System.Collections.Generic;
using Caliburn.Micro;
using GTasksDesktopClient.Core.TasksLists;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Shell
{
    public class DataContext
    {
        private readonly IEventAggregator _eventAggregator;

        public string SelectedTasksListId { get; set; }

        private IEnumerable<TaskList> _tasksLists;
        public IEnumerable<TaskList> TasksLists
        {
            get { return _tasksLists; }
            set
            {
                _tasksLists = value;
                _eventAggregator.Publish(new TasksListsUpdated(TasksLists));
            }
        }
        
        public DataContext(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            TasksLists = new List<TaskList>();
        }
    }
}