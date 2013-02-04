using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autofac;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.TasksLists.Add;
using GTasksDesktopClient.Core.TasksLists.Details;
using GTasksDesktopClient.Core.TasksLists.Events;
using GTasksDesktopClient.Core.Utils;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists
{
    public class TasksListsViewModel : Screen, ITab, IHandle<TasksListsUpdated>
    {
        private readonly EventAggregator _eventAggregator;
        private readonly CurrentDataContext _currentDataContext;
        private readonly IWindowManager _windowManager;
        private readonly Func<TaskList, TasksListViewModel> _tasksListViewModelFactory;
        private readonly IContainer _container;

        public string Header
        {
            get { return "Listy"; }
        }

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
            CurrentDataContext currentDataContext,
            IWindowManager windowManager,
            Func<TaskList, TasksListViewModel> tasksListViewModelFactory, 
            IContainer container)
        {
            _eventAggregator = eventAggregator;
            _currentDataContext = currentDataContext;
            _windowManager = windowManager;
            _tasksListViewModelFactory = tasksListViewModelFactory;
            _container = container;

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
            TasksLists.Clear();
            
            var tasksListsViewModels = tasksLists.Select(tasksList => _tasksListViewModelFactory(tasksList));
            TasksLists.AddRange(tasksListsViewModels);
            
            UpdateSelectedTasksList();
        }

        private void UpdateSelectedTasksList()
        {
            var lastSelectedTasksList =
                TasksLists.SingleOrDefault(tasksList => tasksList.Id == _currentDataContext.LastLoadedTasksListId);

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
            SelectedTasksList.Load();
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            TasksLists.Clear();
        }

        public void ShowAddNewListView()
        {
            var addTasksListViewModel = _container.Resolve<AddTasksListViewModel>();
            _windowManager.ShowDialog(addTasksListViewModel);
        }

        public void Handle(TasksListsUpdated message)
        {
            UpdateTasksLists(message.TasksLists);
        }
    }
}