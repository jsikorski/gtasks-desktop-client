using System.Collections.Generic;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists.Events
{
    public class TasksListsUpdated
    {
        public IEnumerable<TaskList> TasksLists { get; private set; }

        public TasksListsUpdated(IEnumerable<TaskList> tasksLists)
        {
            TasksLists = tasksLists;
        }
    }
}