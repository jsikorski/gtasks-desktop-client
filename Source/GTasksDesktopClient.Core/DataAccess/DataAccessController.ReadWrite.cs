using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.DataAccess
{
    public partial class DataAccessController
    {
        public class ReadWriteAccess : IDisposable
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

            public ReadWriteAccess(DataAccessController dataAccessController)
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