using Caliburn.Micro;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class SynchronizationStateViewModel : Screen, ISyncStateIndicator
    {
        private SynchronizationState _state;
        public SynchronizationState State
        {
            get { return _state; }
            set
            {
                _state = value;
                NotifyOfPropertyChange(() => State);
            }
        }
    }
}