using GTasksDesktopClient.Core.Lists;

namespace GTasksDesktopClient.Core.Layout
{
    public class LayoutViewModel
    {
        public TasksListsViewModel TasksListsViewModel { get; set; }

        public LayoutViewModel(TasksListsViewModel tasksListsViewModel)
        {
            TasksListsViewModel = tasksListsViewModel;
        }
    }
}