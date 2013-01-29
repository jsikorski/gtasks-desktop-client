namespace GTasksDesktopClient.Core.Synchronization
{
    public interface ISyncStateIndicator
    {
        SynchronizationState State { get; set; }
    }
}