using System;
using System.Globalization;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Tasks
{
    public class TaskViewModel
    {
        private readonly Task _task;

        public string Id
        {
            get { return _task.Id; }
        }

        public bool IsCompleted
        {
            get { return !string.IsNullOrEmpty(_task.Completed); }
            set { _task.Completed = value ? DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) : null; }
        }

        public string Title
        {
            get { return _task.Title; }
            set { _task.Title = Title; }
        }

        public TaskViewModel(Task task)
        {
            _task = task;
        }
    }
}