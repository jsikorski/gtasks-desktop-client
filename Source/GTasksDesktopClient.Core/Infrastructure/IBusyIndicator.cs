namespace GTasksDesktopClient.Core.Infrastructure
{
    public interface IBusyIndicator
    {
        bool IsBusy { get; set; }
        string Message { get; set; }
    }
}