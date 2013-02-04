using System;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Tasks.Update;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Tasks.Details
{
    public class TaskViewModel
    {
        private readonly Task _task;
        private readonly Func<Task, UpdateTask> _updateTaskFactory;

        public string Id
        {
            get { return _task.Id; }
        }

        public bool IsCompleted
        {
            get { return _task.Status == TaskStatus.Completed; }
            set
            {
                if (value)
                {
                    _task.Status = TaskStatus.Completed;
                }
                else
                {
                    _task.Status = TaskStatus.NeedsAction;
                    _task.Completed = null;
                }
            }
        }

        public string Title
        {
            get { return _task.Title; }
            set { _task.Title = Title; }
        }

        public TaskViewModel(
            Task task,
            Func<Task, UpdateTask> updateTaskFactory)
        {
            _task = task;
            _updateTaskFactory = updateTaskFactory;
        }

        public void ToggleCompleted()
        {
            var updateTask = _updateTaskFactory(_task);
            CommandsInvoker.ExecuteCommand(updateTask);
        }
    }
}