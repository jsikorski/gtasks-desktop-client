using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Utils;

namespace GTasksDesktopClient.Core.Tasks
{
    public class TasksViewModel : Screen, ITab, IHandle<TasksUpdated>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<string, ShowTasks> _showTasksFactory;
        private readonly CurrentContext _currentContext;

        public string Header
        {
            get { return "Zadania"; }
        }

        public ObservableCollection<TaskViewModel> Tasks { get; set; }

        public TasksViewModel(
            IEventAggregator eventAggregator, 
            Func<string, ShowTasks> showTasksFactory, 
            CurrentContext currentContext)
        {
            _eventAggregator = eventAggregator;
            _showTasksFactory = showTasksFactory;
            _currentContext = currentContext;

            Tasks = new ObservableCollection<TaskViewModel>();
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);

            var showTasks = _showTasksFactory(_currentContext.SelectedTasksListId);
            CommandsInvoker.ExecuteCommand(showTasks);
        }

        protected override void OnDeactivate(bool close)
        {
            Tasks.Clear();
            _eventAggregator.Unsubscribe(this);
        }

        public void Handle(TasksUpdated message)
        {
            Tasks.Clear();
            var tasksViewModel = message.Tasks.Select(task => new TaskViewModel(task));
            Tasks.AddRange(tasksViewModel);
        }
    }
}