using System.Collections.Generic;
using System.Collections.ObjectModel;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Layout
{
    public class LayoutViewModel
    {
        public ObservableCollection<TaskList> TasksLists { get; private set; }

        public LayoutViewModel(IEnumerable<TaskList> tasksLists)
        {
            TasksLists = new ObservableCollection<TaskList>(tasksLists);
        }
    }
}