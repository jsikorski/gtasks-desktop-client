using System.Collections.Generic;
using System.Threading;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Tasks.Events;
using GTasksDesktopClient.Core.TasksLists.Events;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.DataAccess
{
    public partial class DataContext
    {
        private readonly IEventAggregator _eventAggregator;

        private Semaphore Semaphore { get; set; }
        private string LastTasksETag { get; set; }
        private string LastTasksListsETag { get; set; }
        
        private string _lastLoadedTasksListId;
        private string LastLoadedTasksListId
        {
            get
            {
                return _lastLoadedTasksListId;
            }
            set
            {
                _lastLoadedTasksListId = value;
                if (_lastLoadedTasksListId == null)
                    _eventAggregator.Publish(new SelectedTasksListIdReseted());
            }
        }

        private IEnumerable<TaskList> _tasksLists;
        private IEnumerable<TaskList> TasksLists
        {
            get { return _tasksLists; }
            set
            {
                _tasksLists = value;
                _eventAggregator.Publish(new TasksListsUpdated(TasksLists));
            }
        }

        private IEnumerable<Task> _tasks;
        private IEnumerable<Task> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                _eventAggregator.Publish(new TasksUpdated(value));
            }
        }

        public DataContext(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Semaphore = new Semaphore(1, 1);
            TasksLists = new List<TaskList>();
            Tasks = new List<Task>();
        }

        public ReadWriteAccess GetReadWriteAccess()
        {
            return new ReadWriteAccess(this);
        }

        public ReadAccess GetReadAccess()
        {
            return new ReadAccess(this);
        }
    }
}