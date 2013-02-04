using System.Collections.Generic;
using System.Linq;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.DataAccess
{
    public partial class DataContext
    {
        public class ReadAccess
        {
            private readonly DataContext _dataContext;

            public string LastTasksETag
            {
                get { return _dataContext.LastTasksETag; }
            }

            public string LastTasksListsETag
            {
                get { return _dataContext.LastTasksListsETag; }
            }

            public string LastLoadedTasksListId
            {
                get { return _dataContext.LastLoadedTasksListId; }
            }

            public IEnumerable<TaskList> TasksLists
            {
                get { return _dataContext.TasksLists; }
            }

            public IEnumerable<Task> Tasks
            {
                get { return _dataContext.Tasks; }
            }

            public ReadAccess(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
        }
    }
}