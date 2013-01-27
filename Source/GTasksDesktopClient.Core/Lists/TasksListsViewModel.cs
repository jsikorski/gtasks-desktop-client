using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Synchronization;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Lists
{
    public class TasksListsViewModel : IHandle<ListsUpdated>
    {
        private readonly EventAggregator _eventAggregator;
        public ObservableCollection<TasksListViewModel> TasksLists { get; private set; }

        public TasksListsViewModel(
            EventAggregator eventAggregator,
            CurrentContext currentContext
        )
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            var tasksListsViewModel = currentContext.TasksLists.Select(tasksList => new TasksListViewModel(tasksList));
            TasksLists = new ObservableCollection<TasksListViewModel>(tasksListsViewModel);
        }

        public void Handle(ListsUpdated message)
        {
            TasksLists.Clear();
            message.TasksLists.ToList().ForEach(list => TasksLists.Add(new TasksListViewModel(list)));
        }
    }
}