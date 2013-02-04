using System;
using System.Windows.Input;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Tasks.Edit;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Tasks.Details
{
    public class TaskViewModel : Screen
    {
        private readonly Task _task;
        private readonly Func<Task, EditTask> _editTaskFactory;

        private string _titleBeforeEdit;

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
            set
            {
                _task.Title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        private bool _isBeingEdited;
        public bool IsBeingEdited
        {
            get { return _isBeingEdited; }
            set
            {
                _isBeingEdited = value;
                NotifyOfPropertyChange(() => IsBeingEdited);
            }
        }

        public TaskViewModel(
            Task task,
            Func<Task, EditTask> editTaskFactory)
        {
            _task = task;
            _editTaskFactory = editTaskFactory;
        }

        public void ToggleCompleted()
        {
            var updateTask = _editTaskFactory(_task);
            CommandsInvoker.ExecuteCommand(updateTask);
        }

        public void BeginEditing(MouseButtonEventArgs mouseButtonEventArgs)
        {
            _titleBeforeEdit = _task.Title;
            IsBeingEdited = true;
            mouseButtonEventArgs.Handled = true;
        }

        public void CancelEditing(MouseButtonEventArgs mouseButtonEventArgs)
        {
            Title = _titleBeforeEdit;
            IsBeingEdited = false;
            mouseButtonEventArgs.Handled = true;
        }

        public void Edit(MouseButtonEventArgs mouseButtonEventArgs)
        {
            var editTask = _editTaskFactory(_task);
            CommandsInvoker.ExecuteCommand(editTask);

            mouseButtonEventArgs.Handled = true;
        }
    }
}