using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Lists
{
    public class TasksListsViewModel
    {
        public ObservableCollection<TasksListViewModel> TasksLists { get; private set; }

        public TasksListsViewModel(IEnumerable<TaskList> tasksLists)
        {
            var tasksListsViewModel = tasksLists.Select(tasksList => new TasksListViewModel(tasksList));
            TasksLists = new ObservableCollection<TasksListViewModel>(tasksListsViewModel);
        }
    }
}