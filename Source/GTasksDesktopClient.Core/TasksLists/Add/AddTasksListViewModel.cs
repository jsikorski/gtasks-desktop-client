using System;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure;

namespace GTasksDesktopClient.Core.TasksLists.Add
{
    public class AddTasksListViewModel : Screen
    {
        private const string WindowTitle = "Dodaj listê zadañ";

        private readonly Func<string, AddTasksList> _addTasksListsFactory;
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public bool CanAdd
        {
            get { return !string.IsNullOrEmpty(Title); }
        }

        public AddTasksListViewModel(Func<string, AddTasksList> addTasksListsFactory)
        {
            base.DisplayName = WindowTitle;

            _addTasksListsFactory = addTasksListsFactory;
        }

        public void Add()
        {
            var addTasksList = _addTasksListsFactory(Title);
            CommandsInvoker.ExecuteCommand(addTasksList);

            TryClose();
        }
    }
}