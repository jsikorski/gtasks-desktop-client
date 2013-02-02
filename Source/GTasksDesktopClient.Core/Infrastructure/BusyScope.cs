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

        public void Release()
        {
            _busyIndicator.IsBusy = false;
        }

        public void Dispose()
        {
            Release();
        }
    }
}