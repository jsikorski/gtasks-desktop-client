using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autofac;
using Caliburn.Micro;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Tasks.Add;
using GTasksDesktopClient.Core.Tasks.Details;
using GTasksDesktopClient.Core.Tasks.Events;
using GTasksDesktopClient.Core.TasksLists.Events;
using GTasksDesktopClient.Core.Utils;
using Task = Google.Apis.Tasks.v1.Data.Task;

namespace GTasksDesktopClient.Core.Tasks
{
    public class TasksViewModel : Screen, ITab, IHandle<TasksUpdated>, IHandle<SelectedTasksListIdReseted>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly DataContext _dataContext;
        private readonly IWindowManager _windowManager;
        private readonly Func<Task, TaskViewModel> _taskViewModelFactory;
        private readonly IContainer _container;

        public string Header
        {
            get { return "Zadania"; }
        }

        public ObservableCollection<TaskViewModel> Tasks { get; set; }

        public TasksViewModel(
            IEventAggregator eventAggregator, 
            DataContext dataContext, 
            IWindowManager windowManager,
            Func<Task, TaskViewModel> taskViewModelFactory,
            IContainer container)
        {
            _eventAggregator = eventAggregator;
            _dataContext = dataContext;
            _windowManager = windowManager;
            _taskViewModelFactory = taskViewModelFactory;
            _container = container;

            Tasks = new ObservableCollection<TaskViewModel>();
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
            UpdateTasks(_dataContext.GetReadAccess().Tasks);                
        }

        private void UpdateTasks(IEnumerable<Task> tasks)
        {
            Tasks.Clear();

            if (tasks == null)
                return;

            var tasksViewModels = tasks.Select(task => _taskViewModelFactory(task));
            Tasks.AddRange(tasksViewModels);
        }

        protected override void OnDeactivate(bool close)
        {
            Tasks.Clear();
            _eventAggregator.Unsubscribe(this);
        }

        public void ShowAddNewTaskView()
        {
            var addTaskViewModel = _container.Resolve<AddTaskViewModel>();
            _windowManager.ShowDialog(addTaskViewModel);
        }

        public void Handle(TasksUpdated message)
        {
            UpdateTasks(message.Tasks);
        }

        public void Handle(SelectedTasksListIdReseted message)
        {
            _eventAggregator.Publish(new TasksListsViewRequested());
        }
    }
}