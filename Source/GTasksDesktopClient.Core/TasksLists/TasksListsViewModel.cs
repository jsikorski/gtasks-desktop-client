using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Tasks;
using GTasksDesktopClient.Core.Utils;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists
{
    public class TasksListsViewModel : Screen, ITab, IHandle<TasksListsUpdated>
    {
        private readonly EventAggregator _eventAggregator;
        private readonly Func<string, LoadTasks> _loadTasksFactory;
        private readonly Func<string, DeleteTasksList> _deleteTasksListsFactory;
        private readonly CurrentDataContext _currentDataContext;

        public string Header
        {
            get { return "Listy"; }
        }

        private readonly object _selectedTasksListLock = new object();
        private TasksListViewModel _selectedTasksList;
        public TasksListViewModel SelectedTasksList
        {
            get { return _selectedTasksList; }
            set
            {
                _selectedTasksList = value;
                NotifyOfPropertyChange(() => SelectedTasksList);
            }
        }

        public ObservableCollection<TasksListViewModel> TasksLists { get; private set; }

        public TasksListsViewModel(
            EventAggregator eventAggregator,
            Func<string, LoadTasks> loadTasksFactory,
            Func<string, DeleteTasksList> deleteTasksListsFactory, 
            CurrentDataContext currentDataContext)
        {
            _eventAggregator = eventAggregator;
            _loadTasksFactory = loadTasksFactory;
            _deleteTasksListsFactory = deleteTasksListsFactory;
            _currentDataContext = currentDataContext;

            TasksLists = new ObservableCollection<TasksListViewModel>();
            SelectFirstTasksList();
        }

        protected override void OnActivate()
        {
            UpdateTasksLists(_currentDataContext.TasksLists);
            _eventAggregator.Subscribe(this);
        }

        private void UpdateTasksLists(IEnumerable<TaskList> tasksLists)
        {
            lock (_selectedTasksListLock)
            {
                TasksLists.Clear();
                var tasksListsViewModels = tasksLists.Select(tasksList => new TasksListViewModel(tasksList));
                TasksLists.AddRange(tasksListsViewModels);
                UpdateSelectedTasksList();
            }
        }

        private void UpdateSelectedTasksList()
        {
            var lastSelectedTasksList =
                TasksLists.SingleOrDefault(tasksList => tasksList.Id == _currentDataContext.SelectedTasksListId);

            if (lastSelectedTasksList == null)
                SelectFirstTasksList();
            else
                SelectedTasksList = lastSelectedTasksList;
        }

        private void SelectFirstTasksList()
        {
            if (!TasksLists.Any())
                return;

            SelectedTasksList = TasksLists.First();
            LoadTasks(SelectedTasksList.Id);
        }

        private void LoadTasks(string tasksListId)
        {
            var loadTasks = _loadTasksFactory(tasksListId);
            CommandsInvoker.ExecuteCommand(loadTasks);
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            TasksLists.Clear();
        }

        public void ShowTasksList()
        {
            string tasksListId = GetSelectedTasksListId();
            if (string.IsNullOrEmpty(tasksListId))
                return;

            _eventAggregator.Publish(new TasksViewRequested());
            LoadTasks(tasksListId);
        }

        private string GetSelectedTasksListId()
        {
            lock (_selectedTasksListLock)
            {
                if (SelectedTasksList == null)
                    return null;

                return SelectedTasksList.Id;
            }
        }

        public void DeleteTasksList(MouseButtonEventArgs mouseButtonEventArgs)
        {
            string tasksListId = GetSelectedTasksListId();
            if (string.IsNullOrEmpty(tasksListId))
                return;

            var deleteTasksLists = _deleteTasksListsFactory(SelectedTasksList.Id);
            CommandsInvoker.ExecuteCommand(deleteTasksLists);

            mouseButtonEventArgs.Handled = true;
        }

        public void Handle(TasksListsUpdated message)
        {
            UpdateTasksLists(message.TasksLists);
        }
    }
}