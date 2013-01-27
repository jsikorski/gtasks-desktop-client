using System.Collections.Generic;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Lists
{
    public class ListsUpdated
    {
        public IEnumerable<TaskList> TasksLists { get; private set; }

        public ListsUpdated(IEnumerable<TaskList> tasksLists)
        {
            TasksLists = tasksLists;
        }
    }
}