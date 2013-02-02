namespace GTasksDesktopClient.Core.Infrastructure
{
    public interface IBusyIndicator
    {
        bool IsBusy { get; set; }
    }
}