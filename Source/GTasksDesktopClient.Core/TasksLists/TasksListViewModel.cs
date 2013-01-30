using System;
using System.Globalization;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists
{
    public class TasksListViewModel
    {
        private readonly TaskList _taskList;

        public string Id
        {
            get { return _taskList.Id; }
        }

        public string Title
        {
            get { return _taskList.Title; }
            set { _taskList.Title = value; }
        }

        public DateTime LastModifiedAt
        {
            get { return Convert.ToDateTime(_taskList.Updated); }
            set { _taskList.Updated = value.ToString(CultureInfo.InvariantCulture); }
        }

        public TasksListViewModel(TaskList taskList)
        {
            _taskList = taskList;
        }
    }
}