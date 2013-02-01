using System.Threading;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class SynchronizationContext
    {
        private readonly Semaphore _semaphore;

        public string LastTasksListsETag { get; set; }

        public SynchronizationContext()
        {
            _semaphore = new Semaphore(1, 1);
        }

        public void Lock()
        {
            _semaphore.WaitOne();
        }

        public void Unlock()
        {
            _semaphore.Release();
        }
    }
}