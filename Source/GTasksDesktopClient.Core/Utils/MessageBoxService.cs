using Caliburn.Micro;
using Xceed.Wpf.Toolkit;

namespace GTasksDesktopClient.Core.Utils
{
    public static class MessageBoxService
    {
        public static void ShowInfo(string message)
        {
            Show(message, "Informacja");
        }

        public static void ShowError(string message)
        {
            Show(message, "Błąd");
        }

        public static void Show(string message, string header)
        {
            Execute.OnUIThread(() => MessageBox.Show(message, header));
        }
    }
}