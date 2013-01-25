using System.Collections.Generic;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class ListsUpdated
    {
        public IEnumerable<TaskList> TasksLists { get; set; } 
    }
}