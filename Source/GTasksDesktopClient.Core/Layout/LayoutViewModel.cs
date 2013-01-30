using Caliburn.Micro;
using GTasksDesktopClient.Core.Synchronization;
using GTasksDesktopClient.Core.Tasks;
using GTasksDesktopClient.Core.TasksLists;

namespace GTasksDesktopClient.Core.Layout
{
    public class LayoutViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public TasksListsViewModel TasksListsViewModel { get; set; }
        public TasksViewModel TasksViewModel { get; set; }
        public SynchronizationStateViewModel SynchronizationStateViewModel { get; set; }

        public LayoutViewModel(
            TasksListsViewModel tasksListsViewModel, 
            TasksViewModel tasksViewModel,
            SynchronizationStateViewModel synchronizationStateViewModel)
        {
            TasksListsViewModel = tasksListsViewModel;
            TasksViewModel = tasksViewModel;
            SynchronizationStateViewModel = synchronizationStateViewModel;
        }
    }
}