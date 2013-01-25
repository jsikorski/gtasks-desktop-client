using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GTasksDesktopClient.Core.Synchronization;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Lists
{
    public class TasksListsViewModel
    {
        public ObservableCollection<TasksListViewModel> TasksLists { get; private set; }

        public TasksListsViewModel(SynchronizationContext synchronizationContext)
        {
            var tasksListsViewModel = synchronizationContext.TasksLists.Select(tasksList => new TasksListViewModel(tasksList));
            TasksLists = new ObservableCollection<TasksListViewModel>(tasksListsViewModel);
        }
    }
}