using System;
using System.Collections.Generic;
using Autofac;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Shell;

namespace GTasksDesktopClient.Core
{
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        private IContainer _container;

        protected override void Configure()
        {
            _container = InitializeContainer();
        }

        private IContainer InitializeContainer()
        {
            var containerBuilder = new ContainerBuilder();
            return containerBuilder.Build();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.Resolve(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.Resolve(service.MakeArrayType()) as IEnumerable<object>;
        }
    }
}