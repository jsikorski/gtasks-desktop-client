using System;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class SynchronizationContext
    {
        private readonly Semaphore _semaphore;

        public string LastTasksETag { get; set; }
        public string LastTasksListsETag { get; set; }

        public SynchronizationContext()
        {
            _semaphore = new Semaphore(1, 1);
        }

        //public SynchronizationData GetData()
        //{
            
        //}

        public void Lock()
        {
            _semaphore.WaitOne();
        }

        public void Unlock()
        {
            _semaphore.Release();
        }

        //public class SynchronizationData : IDisposable
        //{
        //    private SynchronizationData()
        //    {
        //    }
 
        //    public static SynchronizationData CreateFor(SynchronizationContext synchronizationContext)
        //    {
                
        //    }

            
        //}
    }
}