﻿using Caliburn.Micro;
using GTasksDesktopClient.Core.Settings;
using GTasksDesktopClient.Core.Synchronization;
using GTasksDesktopClient.Core.Tasks;
using GTasksDesktopClient.Core.Tasks.Events;
using GTasksDesktopClient.Core.TasksLists;
using GTasksDesktopClient.Core.TasksLists.Events;

namespace GTasksDesktopClient.Core.Layout
{
    public class LayoutViewModel : Conductor<ITab>.Collection.OneActive, 
        IHandle<TasksListsViewRequested>, IHandle<TasksViewRequested>
    {
        private readonly EventAggregator _eventAggregator;

        private TasksListsViewModel TasksListsViewModel { get; set; }
        private TasksViewModel TasksViewModel { get; set; }        
        public SynchronizationStateViewModel SynchronizationStateViewModel { get; private set; }

        public LayoutViewModel(
            EventAggregator eventAggregator,
            TasksListsViewModel tasksListsViewModel, 
            TasksViewModel tasksViewModel,
            SettingsViewModel settingsViewModel,
            SynchronizationStateViewModel synchronizationStateViewModel)
        {
            _eventAggregator = eventAggregator;
            TasksListsViewModel = tasksListsViewModel;
            TasksViewModel = tasksViewModel;

            Items.Add(TasksListsViewModel);
            Items.Add(TasksViewModel);
            Items.Add(settingsViewModel);
            
            SynchronizationStateViewModel = synchronizationStateViewModel;
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
        }

        public void Handle(TasksListsViewRequested message)
        {
            ActivateItem(TasksListsViewModel);
        }

        public void Handle(TasksViewRequested message)
        {
            ActivateItem(TasksViewModel);
        }
    }
}