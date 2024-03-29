using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Utils;

namespace GTasksDesktopClient.Core
{
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        private IContainer _container;

        protected override void Configure()
        {
            _container = AutofacInitializer.InitializeContainer();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.Resolve(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.Resolve(service.MakeArrayType()) as IEnumerable<object>;
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            _container.Resolve<BackgroundTasksManager>().StartAll();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _container.Resolve<BackgroundTasksManager>().StopAll();
            base.OnExit(sender, e);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _container.Resolve<BackgroundTasksManager>().StopAll();
            MessageBoxService.ShowError("Wyst�pi� nieznany b��d.");
            Application.Current.Shutdown();
        }
    }
}