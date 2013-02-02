using System.Collections.Generic;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Tasks;
using GTasksDesktopClient.Core.TasksLists;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Shell
{
    public class CurrentDataContext
    {
        private readonly IEventAggregator _eventAggregator;

        private string _selectedTasksListId;
        public string SelectedTasksListId
        {
            get
            {
                return _selectedTasksListId;
            }
            set
            {
                _selectedTasksListId = value;
                if (_selectedTasksListId == null)
                    _eventAggregator.Publish(new SelectedTasksListIdReseted());
            }
        }

        private IEnumerable<Task> _tasks;
        public IEnumerable<Task> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                _eventAggregator.Publish(new TasksUpdated(value));
            }
        }

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
        
        public CurrentDataContext(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            Tasks = new List<Task>();
            TasksLists = new List<TaskList>();
        }
    }
}