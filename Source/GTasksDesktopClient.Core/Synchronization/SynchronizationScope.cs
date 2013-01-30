using System;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class SynchronizationScope : IDisposable
    {
        private readonly ISyncStateIndicator _syncStateIndicator;

        public SynchronizationScope(ISyncStateIndicator syncStateIndicator)
        {
            _syncStateIndicator = syncStateIndicator;
            _syncStateIndicator.State = SynchronizationState.Connecting;
        }

        public void Dispose()
        {
            _syncStateIndicator.State = SynchronizationState.Idle;    
        }
    }
}