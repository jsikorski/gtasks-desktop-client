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
            _eventAggregator.Subscribe(this);

            TasksListsViewModel = tasksListsViewModel;
            TasksViewModel = tasksViewModel;

            Items.Add(TasksListsViewModel);
            Items.Add(TasksViewModel);
            
            SynchronizationStateViewModel = synchronizationStateViewModel;
        }

        public void Handle(TasksViewRequested message)
        {
            ShowTasks();
        }

        private void ShowTasks()
        {
            ActivateItem(TasksViewModel);
        }
    }
}