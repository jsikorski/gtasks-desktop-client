using Caliburn.Micro;
using GTasksDesktopClient.Core.Layout;

namespace GTasksDesktopClient.Core.Settings
{
    public class SettingsViewModel : Screen, ITab
    {
        public string Header
        {
            get { return "Ustawienia"; }
        }

        public bool BackgroundTasksEnabled
        {
            get { return Properties.Settings.Default.BackgroundTasksEnabled; }
            set { Properties.Settings.Default.BackgroundTasksEnabled = value; }
        }

        public int BackgroundTasksFrequency
        {
            get { return Properties.Settings.Default.BackgroundTasksFrequency; }
            set
            {
                Properties.Settings.Default.BackgroundTasksFrequency = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}