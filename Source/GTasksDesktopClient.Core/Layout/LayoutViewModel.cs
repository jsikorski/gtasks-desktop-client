using GTasksDesktopClient.Core.Lists;
using GTasksDesktopClient.Core.Synchronization;

namespace GTasksDesktopClient.Core.Layout
{
    public class LayoutViewModel
    {
        public TasksListsViewModel TasksListsViewModel { get; set; }
        public SynchronizationStateViewModel SynchronizationStateViewModel { get; set; }

        public LayoutViewModel(
            TasksListsViewModel tasksListsViewModel, 
            SynchronizationStateViewModel synchronizationStateViewModel)
        {
            TasksListsViewModel = tasksListsViewModel;
            SynchronizationStateViewModel = synchronizationStateViewModel;
        }
    }
}