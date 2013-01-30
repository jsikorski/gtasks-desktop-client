using System.Collections.Generic;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Tasks
{
    public class TasksUpdated
    {
        public IEnumerable<Task> Tasks { get; private set; }

        public TasksUpdated(IEnumerable<Task> tasks)
        {
            Tasks = tasks;
        }
    }
}