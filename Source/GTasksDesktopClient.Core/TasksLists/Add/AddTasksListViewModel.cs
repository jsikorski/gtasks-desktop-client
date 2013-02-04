using System;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.TasksLists.Add
{
    public class AddTasksListViewModel : Screen
    {
        private const string WindowTitle = "Dodaj listê zadañ";

        private readonly Func<TaskList, AddTasksList> _addTasksListsFactory;
        private readonly TaskList _task;

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

        public AddTasksListViewModel(Func<TaskList, AddTasksList> addTasksListsFactory)
        {
            base.DisplayName = WindowTitle;

            _task = new TaskList();
            _addTasksListsFactory = addTasksListsFactory;
        }

        public void Add()
        {
            var addTasksList = _addTasksListsFactory(_task);
            CommandsInvoker.ExecuteCommand(addTasksList);

            TryClose();
        }
    }
}