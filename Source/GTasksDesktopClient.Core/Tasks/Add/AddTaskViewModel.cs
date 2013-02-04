using System;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Tasks.Add
{
    public class AddTaskViewModel : Screen
    {
        private const string WindowTitle = "Dodaj zadanie";

        private readonly Func<Task, AddTask> _addTaskFactory;
        private readonly Task _task;
        
        public string Title
        {
            get { return _task.Title; }
            set
            {
                _task.Title = value;
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public bool CanAdd
        {
            get { return !string.IsNullOrEmpty(Title); }
        }

        public AddTaskViewModel(Func<Task, AddTask> addTaskFactory)
        {
            base.DisplayName = WindowTitle;

            _task = new Task();
            _addTaskFactory = addTaskFactory;
        }

        public void Add()
        {
            var addTasksList = _addTaskFactory(_task);
            CommandsInvoker.ExecuteCommand(addTasksList);

            TryClose();
        }
    }
}