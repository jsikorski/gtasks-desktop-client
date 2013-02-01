using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Utils;
using Task = Google.Apis.Tasks.v1.Data.Task;

namespace GTasksDesktopClient.Core.Tasks
{
    public class TasksViewModel : Screen, ITab, IHandle<TasksUpdated>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly CurrentDataContext _currentDataContext;

        public string Header
        {
            get { return "Zadania"; }
        }

        public ObservableCollection<TaskViewModel> Tasks { get; set; }

        public TasksViewModel(
            IEventAggregator eventAggregator, 
            CurrentDataContext currentDataContext)
        {
            _eventAggregator = eventAggregator;
            _currentDataContext = currentDataContext;

            Tasks = new ObservableCollection<TaskViewModel>();
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
            UpdateTasks(_currentDataContext.Tasks);
        }

        private void UpdateTasks(IEnumerable<Task> tasks)
        {
            var tasksViewModels = tasks.Select(task => new TaskViewModel(task));
            Tasks.AddRange(tasksViewModels);
        }

        protected override void OnDeactivate(bool close)
        {
            Tasks.Clear();
            _eventAggregator.Unsubscribe(this);
        }

        public void Handle(TasksUpdated message)
        {
            Tasks.Clear();
            UpdateTasks(message.Tasks);
        }
    }
}