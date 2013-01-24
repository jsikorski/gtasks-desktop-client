using System;

namespace GTasksDesktopClient.Core.Infrastructure
{
    public class BusyScopeContext : IDisposable
    {
        private readonly IBusyScope _busyScope;

        public BusyScopeContext(IBusyScope busyScope, string message = "Proszę czekać...")
        {
            _busyScope = busyScope;
            _busyScope.IsBusy = true;
            _busyScope.Message = message;
        }

        public void Dispose()
        {
            _busyScope.IsBusy = false;
        }
    }
}