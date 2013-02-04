using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Tasks;
using GTasksDesktopClient.Core.TasksLists.Events;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core
{
    public class DataAccessController
    {
        private readonly IEventAggregator _eventAggregator;

        private Semaphore Semaphore { get; set; }
        private string LastTasksETag { get; set; }
        private string LastTasksListsETag { get; set; }
        
        private string _lastLoadedTasksListId;
        public string LastLoadedTasksListId
        {
            get
            {
                return _lastLoadedTasksListId;
            }
            private set
            {
                _lastLoadedTasksListId = value;
                if (_lastLoadedTasksListId == null)
                    _eventAggregator.Publish(new SelectedTasksListIdReseted());
            }
        }

        private IEnumerable<TaskList> _tasksLists;
        public IEnumerable<TaskList> TasksLists
        {
            get { return _tasksLists; }
            private set
            {
                _tasksLists = value;
                _eventAggregator.Publish(new TasksListsUpdated(TasksLists));
            }
        }

        private IEnumerable<Task> _tasks;
        public IEnumerable<Task> Tasks
        {
            get { return _tasks; }
            private set
            {
                _tasks = value;
                _eventAggregator.Publish(new TasksUpdated(value));
            }
        }

        public DataAccessController(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Semaphore = new Semaphore(1, 1);
            TasksLists = new List<TaskList>();
            Tasks = new List<Task>();
        }

        public DataContext GetContext()
        {
            return new DataContext(this);
        }


        public class DataContext : IDisposable
        {
            private readonly DataAccessController _dataAccessController;

            public string LastTasksETag
            {
                get { return _dataAccessController.LastTasksETag; }
                set { _dataAccessController.LastTasksETag = value; }
            }

            public string LastTasksListsETag
            {
                get { return _dataAccessController.LastTasksListsETag; }
                set { _dataAccessController.LastTasksListsETag = value; }
            }

            public string LastLoadedTasksListId
            {
                get { return _dataAccessController.LastLoadedTasksListId; }
                private set { _dataAccessController.LastLoadedTasksListId = value; }
            }

            public IEnumerable<TaskList> TasksLists
            {
                get { return _dataAccessController.TasksLists; }
                set { _dataAccessController.TasksLists = value; }
            }

            public IEnumerable<Task> Tasks
            {
                get { return _dataAccessController.Tasks; }
                set { _dataAccessController.Tasks = value; }
            }

            public bool TasksListExists(string tasksListId)
            {
                return TasksLists.Any(tasksList => tasksList.Id == tasksListId);
            }

            public DataContext(DataAccessController dataAccessController)
            {
                _dataAccessController = dataAccessController;
                _dataAccessController.Semaphore.WaitOne();
            }

            public void UpdateTasksLists(TasksService tasksService)
            {
                var tasksLists = tasksService.Tasklists.List().Fetch();

                if (tasksLists.ETag != LastTasksListsETag)
                {
                    TasksLists = tasksLists.Items;
                    LastTasksListsETag = tasksLists.ETag;
                }
            }

            public void UpdateTasks(TasksService tasksService, string tasksListId)
            {
                var tasks = tasksService.Tasks.List(tasksListId).Fetch();
                
                if (tasks.ETag != LastTasksETag || LastLoadedTasksListId != tasksListId)
                {
                    Tasks = tasks.Items;
                    LastTasksETag = tasks.ETag;
                    LastLoadedTasksListId = tasksListId;
                }
            }

            public void ValidateLastLoadedTasksListsId()
            {
                var isTasksListStillPresent = TasksListExists(LastLoadedTasksListId);

                if (!isTasksListStillPresent)
                    LastLoadedTasksListId = null;
            }

            public void Dispose()
            {
                _dataAccessController.Semaphore.Release();
            }
        }
    }
}