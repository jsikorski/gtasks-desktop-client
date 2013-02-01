﻿using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;
using GTasksDesktopClient.Core.Shell;
using Google.Apis.Tasks.v1;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class Synchronize : IBackgroundTask
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly DataContext _dataContext;
        private readonly TasksService _tasksService;
        private readonly ISyncStateIndicator _syncStateIndicator;

        public Synchronize(
            SynchronizationContext synchronizationContext,
            DataContext dataContext,
            TasksService tasksService, 
            ISyncStateIndicator syncStateIndicator)
        {
            _synchronizationContext = synchronizationContext;
            _dataContext = dataContext;
            _tasksService = tasksService;
            _syncStateIndicator = syncStateIndicator;
        }

        public void Execute()
        {
            _synchronizationContext.Lock();

            using (new SynchronizationScope(_syncStateIndicator))
            {
                var lists = _tasksService.Tasklists.List().Fetch();

                if (_synchronizationContext.LastTasksListsETag != lists.ETag)
                {
                    _dataContext.TasksLists = lists.Items;
                    _synchronizationContext.LastTasksListsETag = lists.ETag;
                }
            }
        
            _synchronizationContext.Unlock();
        }
    }
}