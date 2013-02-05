using Autofac;
using Caliburn.Micro;
using GApiHelpers.Authorization;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;

namespace GTasksDesktopClient.Core.Settings
{
    public class SettingsViewModel : Screen, ITab
    {
        private readonly AuthorizationManager _authorizationManager;
        private readonly IContainer _container;

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

        public SettingsViewModel(AuthorizationManager authorizationManager, IContainer container)
        {
            _authorizationManager = authorizationManager;
            _container = container;
        }

        public void LogoutAndExit()
        {
            var logoutAndExit = _container.Resolve<LogoutAndExit>();
            CommandsInvoker.ExecuteCommand(logoutAndExit);
        }
    }
}