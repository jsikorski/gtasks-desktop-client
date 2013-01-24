namespace GTasksDesktopClient.Core.Infrastructure
{
    public interface IBusyScope
    {
        bool IsBusy { get; set; }
        string Message { get; set; }
    }
}