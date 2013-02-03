using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Utils;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists
{
    public class TasksListsViewModel : Screen, ITab, IHandle<TasksListsUpdated>
    {
        private readonly EventAggregator _eventAggregator;
        private readonly CurrentDataContext _currentDataContext;
        private readonly Func<TaskList, TasksListViewModel> _tasksListViewModelFactory;

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
            Func<TaskList, TasksListViewModel> tasksListViewModelFactory)
        {
            _eventAggregator = eventAggregator;
            _currentDataContext = currentDataContext;
            _tasksListViewModelFactory = tasksListViewModelFactory;

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
            var addedLists = TasksLists.Where(list => list.IsBeingAdded).ToList();
            TasksLists.Clear();
            var tasksListsViewModels = tasksLists.Select(tasksList => _tasksListViewModelFactory(tasksList));
            TasksLists.AddRange(tasksListsViewModels);
            TasksLists.AddRange(addedLists);
            UpdateSelectedTasksList();
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
            SelectedTasksList.Load();
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            TasksLists.Clear();
        }

        public void BeginAddingList()
        {
            var tasksList = _tasksListViewModelFactory(new TaskList());
            tasksList.IsBeingAdded = true;
            TasksLists.Add(tasksList);
            SelectedTasksList = tasksList;
        }

        public void Handle(TasksListsUpdated message)
        {
            UpdateTasksLists(message.TasksLists);
        }
    }
}