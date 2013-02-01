using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Tasks;
using GTasksDesktopClient.Core.Utils;
using DotNetOpenAuth.Messaging;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists
{
    public class TasksListsViewModel : Screen, ITab, IHandle<TasksListsUpdated>
    {
        private readonly EventAggregator _eventAggregator;
        private readonly CurrentDataContext _currentDataContext;

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
                if (value == null)
                    return;

                _selectedTasksList = value;
                _currentDataContext.SelectedTasksListId = _selectedTasksList.Id;
                NotifyOfPropertyChange(() => SelectedTasksList);
            }
        }

        public ObservableCollection<TasksListViewModel> TasksLists { get; private set; }

        public TasksListsViewModel(
            EventAggregator eventAggregator,
            CurrentDataContext currentDataContext)
        {
            _eventAggregator = eventAggregator;
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
            var tasksListsViewModels = tasksLists.Select(tasksList => new TasksListViewModel(tasksList));
            TasksLists.AddRange(tasksListsViewModels);
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
            if (TasksLists.Any())
                SelectedTasksList = TasksLists.First();
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            TasksLists.Clear();
        }

        public void ShowTasksList()
        {
            _eventAggregator.Publish(new TasksViewRequested());
        }

        public void Handle(TasksListsUpdated message)
        {
            TasksLists.Clear();
            UpdateTasksLists(message.TasksLists);
        }
    }
}