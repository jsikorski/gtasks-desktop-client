using System.Collections.Generic;
using System.Collections.ObjectModel;
using GTasksDesktopClient.Core.Lists;
using Google.Apis.Tasks.v1.Data;
using System.Linq;

namespace GTasksDesktopClient.Core.Layout
{
    public class LayoutViewModel
    {
        public ObservableCollection<TasksListViewModel> TasksLists { get; private set; }

        public LayoutViewModel(IEnumerable<TaskList> tasksLists)
        {
            var tasksListsViewModel = tasksLists.Select(tasksList => new TasksListViewModel(tasksList));
            TasksLists = new ObservableCollection<TasksListViewModel>(tasksListsViewModel);
        }
    }
}