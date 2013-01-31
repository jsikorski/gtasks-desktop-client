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

        public string Header
        {
            get { return "Listy"; }
        }

        public TasksListViewModel SelectedTasksList { get; set; }
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

            var tasksListsViewModels = currentContext.TasksLists.Select(tasksList => new TasksListViewModel(tasksList));
            TasksLists = new ObservableCollection<TasksListViewModel>(tasksListsViewModels);
        }

        public void ShowTasksList()
        {
            _eventAggregator.Publish(new TasksViewRequested());

            var showTasks = _showTasksFactory(SelectedTasksList.Id);
            CommandsInvoker.ExecuteCommand(showTasks);
        }

        public void Handle(TasksListsUpdated message)
        {
            TasksLists.Clear();
            message.TasksLists.ToList().ForEach(tasksList => TasksLists.Add(new TasksListViewModel(tasksList)));
        }
    }
}