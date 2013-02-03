﻿using System;
using System.Globalization;
using System.Windows.Input;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Tasks;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists
{
    public class TasksListViewModel : Screen
    {
        private readonly TaskList _taskList;
        private readonly EventAggregator _eventAggregator;
        private readonly Func<string, LoadTasks> _loadTasksFactory;
        private readonly Func<TaskList, EditTasksList> _editTasksListFactory;
        private readonly Func<string, DeleteTasksList> _deleteTasksListsFactory;

        public string Id
        {
            get { return _taskList.Id; }
        }

        public string Title
        {
            get { return _taskList.Title; }
            set
            {
                _taskList.Title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        public DateTime LastModifiedAt
        {
            get { return Convert.ToDateTime(_taskList.Updated); }
            set { _taskList.Updated = value.ToString(CultureInfo.InvariantCulture); }
        }

        private string _titleBeforeEdit;

        private bool _isInEditMode;
        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            private set
            {
                _isInEditMode = value;
                NotifyOfPropertyChange(() => IsInEditMode);
            }
        }

        public TasksListViewModel(
            TaskList taskList, 
            EventAggregator eventAggregator,
            Func<string, LoadTasks> loadTasksFactory, 
            Func<TaskList, EditTasksList> editTasksListFactory,
            Func<string, DeleteTasksList> deleteTasksListsFactory)
        {
            _taskList = taskList;
            _eventAggregator = eventAggregator;
            _loadTasksFactory = loadTasksFactory;
            _editTasksListFactory = editTasksListFactory;
            _deleteTasksListsFactory = deleteTasksListsFactory;
        }

        public void Show()
        {
            _eventAggregator.Publish(new TasksViewRequested());
            Load();
        }

        public void Load()
        {
            var loadTasks = _loadTasksFactory(Id);
            CommandsInvoker.ExecuteCommand(loadTasks);
        }

        public void BeginEditing(MouseButtonEventArgs mouseButtonEventArgs)
        {
            _titleBeforeEdit = Title;
            IsInEditMode = true;
            mouseButtonEventArgs.Handled = true;
        }

        public void CancelEditing(MouseButtonEventArgs mouseButtonEventArgs)
        {
            Title = _titleBeforeEdit;
            IsInEditMode = false;
            mouseButtonEventArgs.Handled = true;
        }

        public void Edit(MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (string.IsNullOrEmpty(Title))
                return;

            if (Title != _titleBeforeEdit)
            {
                var editTasksList = _editTasksListFactory(_taskList);
                CommandsInvoker.ExecuteCommand(editTasksList);
            }

            IsInEditMode = false;
            mouseButtonEventArgs.Handled = true;
        }

        public void Delete(MouseButtonEventArgs mouseButtonEventArgs)
        {
            var deleteTasksLists = _deleteTasksListsFactory(Id);
            CommandsInvoker.ExecuteCommand(deleteTasksLists);
            mouseButtonEventArgs.Handled = true;
        }
    }
}