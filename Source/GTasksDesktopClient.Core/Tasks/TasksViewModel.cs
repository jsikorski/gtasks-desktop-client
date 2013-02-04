using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Tasks.Details;
using GTasksDesktopClient.Core.Tasks.Events;
using GTasksDesktopClient.Core.TasksLists;
using GTasksDesktopClient.Core.TasksLists.Events;
using GTasksDesktopClient.Core.Utils;
using Task = Google.Apis.Tasks.v1.Data.Task;

namespace GTasksDesktopClient.Core.Tasks
{
    public class TasksViewModel : Screen, ITab, IHandle<TasksUpdated>, IHandle<SelectedTasksListIdReseted>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly DataAccessController _dataAccessController;

        public string Header
        {
            get { return "Zadania"; }
        }

        public ObservableCollection<TaskViewModel> Tasks { get; set; }

        public TasksViewModel(
            IEventAggregator eventAggregator, 
            DataAccessController dataAccessController)
        {
            _eventAggregator = eventAggregator;
            _dataAccessController = dataAccessController;

            Tasks = new ObservableCollection<TaskViewModel>();
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
            UpdateTasks(_dataAccessController.GetReadAccess().Tasks);                
        }

        private void UpdateTasks(IEnumerable<Task> tasks)
        {
            Tasks.Clear();
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
            UpdateTasks(message.Tasks);
        }

        public void Handle(SelectedTasksListIdReseted message)
        {
            _eventAggregator.Publish(new TasksListsViewRequested());
        }
    }
}