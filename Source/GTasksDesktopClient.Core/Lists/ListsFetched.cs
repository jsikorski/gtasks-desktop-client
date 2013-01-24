using System.Collections.Generic;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Lists
{
    public class ListsFetched
    {
        public IEnumerable<TaskList> TasksLists { get; set; } 
    }
}