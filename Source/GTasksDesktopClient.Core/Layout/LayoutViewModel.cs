using Caliburn.Micro;
using GTasksDesktopClient.Core.Synchronization;
using GTasksDesktopClient.Core.Tasks;
using GTasksDesktopClient.Core.TasksLists;

namespace GTasksDesktopClient.Core.Layout
{
    public class LayoutViewModel : Conductor<ITab>.Collection.OneActive, IHandle<TasksViewRequested>
    {
        private readonly EventAggregator _eventAggregator;

        private TasksListsViewModel TasksListsViewModel { get; set; }
        private TasksViewModel TasksViewModel { get; set; }        
        public SynchronizationStateViewModel SynchronizationStateViewModel { get; private set; }

        public LayoutViewModel(
            EventAggregator eventAggregator,
            TasksListsViewModel tasksListsViewModel, 
            TasksViewModel tasksViewModel,
            SynchronizationStateViewModel synchronizationStateViewModel)
        {
            _eventAggregator = eventAggregator;
            TasksListsViewModel = tasksListsViewModel;
            TasksViewModel = tasksViewModel;

            Items.Add(TasksListsViewModel);
            Items.Add(TasksViewModel);
            
            SynchronizationStateViewModel = synchronizationStateViewModel;
        }

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
        }

        public void Handle(TasksViewRequested message)
        {
            ActivateItem(TasksViewModel);
        }
    }
}