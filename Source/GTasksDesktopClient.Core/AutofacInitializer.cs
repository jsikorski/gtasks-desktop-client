using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Caliburn.Micro;
using GApiHelpers.Authorization;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;
using GTasksDesktopClient.Core.Shell;
using GTasksDesktopClient.Core.Synchronization;
using Google.Apis.Tasks.v1;
using IStartable = GTasksDesktopClient.Core.Infrastructure.IStartable;

namespace GTasksDesktopClient.Core
{
    public static class AutofacInitializer
    {
        private static IContainer _container;

        public static IContainer InitializeContainer()
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

            _container = containerBuilder.Build();
            return _container;
        }

        private static void RegisterViews(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.Name.EndsWith("View"));
        }

        private static void RegisterViewModels(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.Name.EndsWith("ViewModel"))
                            .Where(type => !IsIndicator(type));
        }

        private static bool IsIndicator(Type type)
        {
            return type.GetInterfaces().Any(i => i.Name.EndsWith("Indicator"));
        }

        private static void RegisterIndicators(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(IsIndicator)
                            .SingleInstance()
                            .AsSelf()
                            .AsImplementedInterfaces();
        }

        private static void RegisterCommands(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.GetInterfaces().Any(i => i == typeof(ICommand)))
                            .AsSelf();
        }

        private static void RegisterStartables(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.GetInterfaces().Any(i => i == typeof(IStartable)))
                            .AsImplementedInterfaces();
        }

        private static void RegisterStopables(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                            .Where(type => type.GetInterfaces().Any(i => i == typeof(IStopable)))
                            .AsImplementedInterfaces();
        }

        private static void RegisterBackgroundTasks(ContainerBuilder containerBuilder)
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

        private static void RegisterApplicationServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterInstance(new AuthorizationManager(Authorization.GetConfiguration()));
            containerBuilder.Register(
                context => new TasksService(context.Resolve<AuthorizationManager>().GetAuthenticator())).As<TasksService>();
        }

        private static void RegisterContexts(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<BackgroundTasksContext>().SingleInstance();
            containerBuilder.RegisterType<CurrentDataContext>().SingleInstance();
            containerBuilder.RegisterType<SynchronizationContext>().SingleInstance();
        }
    }
}