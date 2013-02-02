using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Caliburn.Micro;
using GApiHelpers.Authorization;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;
using GTasksDesktopClient.Core.Shell;
using System.Linq;
using GTasksDesktopClient.Core.Synchronization;
using Google.Apis.Tasks.v1;
using Google.Apis.Util;
using IStartable = GTasksDesktopClient.Core.Infrastructure.IStartable;

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

            var startables = _container.Resolve<IEnumerable<IStartable>>();
            startables.ToList().ForEach(startable => startable.Start());
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            var stopables = _container.Resolve<IEnumerable<IStopable>>();
            stopables.ToList().ForEach(stopable => stopable.Stop());

            base.OnExit(sender, e);
        }
    }
}