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
            _container = InitializeContainer();
        }
        
        private IContainer InitializeContainer()
        {
            var containerBuilder = new ContainerBuilder();

            RegisterViews(containerBuilder);
            RegisterViewModels(containerBuilder);
            RegisterIndicators(containerBuilder);
            RegisterCommands(containerBuilder);
            RegisterStartables(containerBuilder);
            RegisterStopables(containerBuilder);
            RegisterBackgroundTasks(containerBuilder);
            RegisterCaliburnComponents(containerBuilder);
            RegisterApplicationServices(containerBuilder);
            RegisterContexts(containerBuilder);
            containerBuilder.Register(_ => _container).As<IContainer>();

            return containerBuilder.Build();
        }

        private void RegisterViews(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.Name.EndsWith("View"));
        }

        private void RegisterViewModels(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.Name.EndsWith("ViewModel"))
                            .Where(type => !IsIndicator(type));
        }

        private bool IsIndicator(Type type)
        {
            return type.GetInterfaces().Any(i => i.Name.EndsWith("Indicator"));
        }

        private void RegisterIndicators(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(IsIndicator)
                            .SingleInstance()
                            .AsSelf()
                            .AsImplementedInterfaces();
        }

        private void RegisterCommands(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.GetInterfaces().Any(i => i == typeof (ICommand)))
                            .AsSelf();
        }

        private void RegisterStartables(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.GetInterfaces().Any(i => i == typeof(IStartable)))
                            .AsImplementedInterfaces();
        }

        private void RegisterStopables(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.GetInterfaces().Any(i => i == typeof(IStopable)))
                            .AsImplementedInterfaces();
        }

        private void RegisterBackgroundTasks(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.GetInterfaces().Any(i => i == typeof(IBackgroundTask)))
                            .AsImplementedInterfaces();
        }

        private static void RegisterCaliburnComponents(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<WindowManager>().SingleInstance().AsSelf().AsImplementedInterfaces();
            containerBuilder.RegisterType<EventAggregator>().SingleInstance().AsSelf().AsImplementedInterfaces();
        }

        private void RegisterApplicationServices(ContainerBuilder containerBuilder)
        {
            var scope = TasksService.Scopes.Tasks.GetStringValue();
            var scopes = new[] { scope };

            var authorizationConfig = new AuthorizationConfig
                {
                    ClientIdentifier = Authorization.ClientIdentifier,
                    ClientSecret = Authorization.ClientSecret,
                    Scopes = scopes,
                    RefreshTokenFilePath = Authorization.RefreshTokenFilePath
                };

            AuthorizationManager.Initialize(authorizationConfig);

            containerBuilder.Register(_ => new TasksService(AuthorizationManager.GetAuthenticator())).As<TasksService>();
        }

        private void RegisterContexts(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<BackgroundTasksContext>().SingleInstance();
            containerBuilder.RegisterType<CurrentDataContext>().SingleInstance();
            containerBuilder.RegisterType<SynchronizationContext>().SingleInstance();
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