using System;

namespace GTasksDesktopClient.Core.Infrastructure
{
    public class BusyScope : IDisposable
    {
        private readonly IBusyIndicator _busyIndicator;

        public BusyScope(IBusyIndicator busyIndicator, string message = "Proszę czekać...")
        {
            _busyIndicator = busyIndicator;
            _busyIndicator.IsBusy = true;
            _busyIndicator.Message = message;
        }

        public void Dispose()
        {
            _busyIndicator.IsBusy = false;
        }
    }
}