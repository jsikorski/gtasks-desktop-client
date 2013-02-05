using System.Windows;
using Autofac;
using GApiHelpers.Authorization;
using GTasksDesktopClient.Core.DataAccess;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;

namespace GTasksDesktopClient.Core.Settings
{
    public class LogoutAndExit : ICommand
    {
        private readonly IContainer _container;
        private readonly AuthorizationManager _authorizationManager;
        private readonly IBusyIndicator _busyIndicator;
        private readonly DataContext _dataContext;

        public LogoutAndExit(
            IContainer container,
            AuthorizationManager authorizationManager,
            IBusyIndicator busyIndicator, 
            DataContext dataContext)
        {
            _container = container;
            _authorizationManager = authorizationManager;
            _busyIndicator = busyIndicator;
            _dataContext = dataContext;
        }

        public void Execute()
        {
            using (new BusyScope(_busyIndicator))
            {
                _container.Resolve<BackgroundTasksManager>().StopAll();

                using (var dataAccess = _dataContext.GetReadWriteAccess())
                {
                    dataAccess.ResetAll();
                }

                _authorizationManager.Logout();
            }

            Caliburn.Micro.Execute.OnUIThread(() => Application.Current.Shutdown());
        }
    }
}