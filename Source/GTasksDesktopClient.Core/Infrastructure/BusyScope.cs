using System;

namespace GTasksDesktopClient.Core.Infrastructure
{
    public class BusyScope : IDisposable
    {
        private readonly IBusyIndicator _busyIndicator;

        public BusyScope(IBusyIndicator busyIndicator)
        {
            _busyIndicator = busyIndicator;
            _busyIndicator.IsBusy = true;
        }

        public void Dispose()
        {
            _busyIndicator.IsBusy = false;
        }
    }
}