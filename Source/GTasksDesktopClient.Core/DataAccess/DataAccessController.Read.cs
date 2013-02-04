using System.Collections.Generic;
using System.Linq;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.DataAccess
{
    public partial class DataAccessController
    {
        public class ReadAccess
        {
            private readonly DataAccessController _dataAccessController;

            public string LastTasksETag
            {
                get { return _dataAccessController.LastTasksETag; }
            }

            public string LastTasksListsETag
            {
                get { return _dataAccessController.LastTasksListsETag; }
            }

            public string LastLoadedTasksListId
            {
                get { return _dataAccessController.LastLoadedTasksListId; }
            }

            public IEnumerable<TaskList> TasksLists
            {
                get { return _dataAccessController.TasksLists; }
            }

            public IEnumerable<Task> Tasks
            {
                get { return _dataAccessController.Tasks; }
            }

            public ReadAccess(DataAccessController dataAccessController)
            {
                _dataAccessController = dataAccessController;
            }
        }
    }
}