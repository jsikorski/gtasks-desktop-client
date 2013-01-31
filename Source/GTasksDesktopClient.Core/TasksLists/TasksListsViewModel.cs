using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Tasks;

namespace GTasksDesktopClient.Core.TasksLists
{
    public class TasksListsViewModel : Screen, ITab, IHandle<TasksListsUpdated>
    {
        private readonly EventAggregator _eventAggregator;
        private readonly Func<string, ShowTasks> _showTasksFactory;
        private TasksListViewModel _selectedTasksList;

        public string Header
        {
            get { return "Listy"; }
        }

        public TasksListViewModel SelectedTasksList
        {
            get { return _selectedTasksList; }
            set
            {
                if (value == null)
                    return;

                _selectedTasksList = value;
                NotifyOfPropertyChange(() => SelectedTasksList);
            }
        }

        public ObservableCollection<TasksListViewModel> TasksLists { get; private set; }

        public TasksListsViewModel(
            EventAggregator eventAggregator,
            CurrentContext currentContext,
            Func<string, ShowTasks> showTasksFactory
        )
        {
            _eventAggregator = eventAggregator;
            _showTasksFactory = showTasksFactory;
            _eventAggregator.Subscribe(this);

            var tasksListsViewModel = currentContext.TasksLists.Select(tasksList => new TasksListViewModel(tasksList));
            TasksLists = new ObservableCollection<TasksListViewModel>(tasksListsViewModel);
        }

        public void ShowTasksList()
        {
            _eventAggregator.Publish(new TasksViewRequested());

            var showTasks = _showTasksFactory(_selectedTasksList.Id);
            CommandsInvoker.ExecuteCommand(showTasks);
        }

        public void Handle(TasksListsUpdated message)
        {
            TasksLists.Clear();
            message.TasksLists.ToList().ForEach(list => TasksLists.Add(new TasksListViewModel(list)));

            SelectedTasksList = TasksLists.First();
        }
    }
}