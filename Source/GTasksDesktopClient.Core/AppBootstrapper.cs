using System;
using System.Collections.Generic;
using Autofac;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;
using GTasksDesktopClient.Core.Shell;

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

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            _container.Resolve<BackgroundTasksManager>().StartAll();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _container.Resolve<BackgroundTasksManager>().StopAll();
            base.OnExit(sender, e);
        }
    }
}