using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.DataAccess
{
    public partial class DataContext
    {
        public class ReadWriteAccess : IDisposable
        {
            private readonly DataContext _dataContext;

            public string LastLoadedTasksListId
            {
                get { return _dataContext.LastLoadedTasksListId; }
                private set { _dataContext.LastLoadedTasksListId = value; }
            }

            public string LastTasksETag
            {
                get { return _dataContext.LastTasksETag; }
                set { _dataContext.LastTasksETag = value; }
            }

            public string LastTasksListsETag
            {
                get { return _dataContext.LastTasksListsETag; }
                set { _dataContext.LastTasksListsETag = value; }
            }

            public IEnumerable<TaskList> TasksLists
            {
                get { return _dataContext.TasksLists; }
                set { _dataContext.TasksLists = value; }
            }

            public IEnumerable<Task> Tasks
            {
                get { return _dataContext.Tasks; }
                set { _dataContext.Tasks = value; }
            }

            public bool TasksListExists(string tasksListId)
            {
                return TasksLists.Any(tasksList => tasksList.Id == tasksListId);
            }

            public ReadWriteAccess(DataContext dataContext)
            {
                _dataContext = dataContext;
                _dataContext.Semaphore.WaitOne();
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

            public void ResetAll()
            {
                LastLoadedTasksListId = null;
                LastTasksETag = null;
                LastTasksListsETag = null;
                TasksLists = null;
                Tasks = null;
            }

            public void Dispose()
            {
                _dataContext.Semaphore.Release();
            }
        }
    }
}