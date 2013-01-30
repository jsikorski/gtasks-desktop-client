using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;

namespace GTasksDesktopClient.Core.Tasks
{
    public class TasksViewModel : IHandle<TasksUpdated>
    {
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<TaskViewModel> Tasks { get; set; }

        public TasksViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            Tasks = new ObservableCollection<TaskViewModel>();
            _eventAggregator.Subscribe(this);
        }

        public void Handle(TasksUpdated message)
        {
            Tasks.Clear();
            message.Tasks.ToList().ForEach(task => Tasks.Add(new TaskViewModel(task)));
        }
    }
}