﻿using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;
using GTasksDesktopClient.Core.Shell;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class Synchronize : IBackgroundTask
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly CurrentDataContext _currentDataContext;
        private readonly TasksService _tasksService;
        private readonly ISyncStateIndicator _syncStateIndicator;

        public Synchronize(
            SynchronizationContext synchronizationContext,
            CurrentDataContext currentDataContext,
            TasksService tasksService, 
            ISyncStateIndicator syncStateIndicator)
        {
            _synchronizationContext = synchronizationContext;
            _currentDataContext = currentDataContext;
            _tasksService = tasksService;
            _syncStateIndicator = syncStateIndicator;
        }

        public void Execute()
        {
            _synchronizationContext.Lock();

            using (new SynchronizationScope(_syncStateIndicator))
            {
                SynchronizeLists();
                
                if (IsAnyTasksListSelected())
                    SynchronizeTasks();
            }

            _synchronizationContext.Unlock();
        }


        private void SynchronizeLists()
        {
            var lists = _tasksService.Tasklists.List().Fetch();

            if (_synchronizationContext.LastTasksListsETag != lists.ETag)
            {
                _currentDataContext.TasksLists = lists.Items;
                _synchronizationContext.LastTasksListsETag = lists.ETag;
            }

            UpdateLastLoadedTasksListId();
        }

        private void UpdateLastLoadedTasksListId()
        {
            var isTasksListStillPresent = _currentDataContext
                .TasksListExists(_currentDataContext.LastLoadedTasksListId);

            if (!isTasksListStillPresent)
                _currentDataContext.LastLoadedTasksListId = null;
        }

        private bool IsAnyTasksListSelected()
        {
            return _currentDataContext.LastLoadedTasksListId != null;
        }

        private void SynchronizeTasks()
        {
            var tasks = _tasksService.Tasks.List(_currentDataContext.LastLoadedTasksListId).Fetch();

            if (_synchronizationContext.LastTasksETag != tasks.ETag)
            {
                _currentDataContext.Tasks = tasks.Items;
                _synchronizationContext.LastTasksETag = tasks.ETag;
            }
        }
    }
}