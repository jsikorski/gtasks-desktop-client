using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Authorization;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Lists;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Shell
{
    public class ShellViewModel : Conductor<object>, IBusyScope, IHandle<ListsFetched>
    {
        private const string WindowTitle = "Google Tasks Desktop Client";
        
        private readonly IContainer _container;
        private readonly IEventAggregator _eventAggregator;

        private bool _isBusy;
        private string _message;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        public ShellViewModel(IContainer container, IEventAggregator eventAggregator)
        {
            base.DisplayName = WindowTitle;

            _container = container;
            _eventAggregator = eventAggregator;
            
            _eventAggregator.Subscribe(this);
            AuthorizationManager.AuthorizationRequired += ShowAuthorizationView;
        }

        protected override void OnInitialize()
        {
            var getAllLists = _container.Resolve<GetAllLists>();
            CommandsInvoker.ExecuteCommand(getAllLists);
        }

        private void ShowAuthorizationView(Uri authorizationUrl)
        {
            var authorizationViewModel = _container.Resolve<AuthorizationViewModel>(new TypedParameter(typeof(Uri), authorizationUrl));
            ActivateItem(authorizationViewModel);
        }

        public void Handle(ListsFetched message)
        {
            ShowLayout(message.TasksLists);
        }

        private void ShowLayout(IEnumerable<TaskList> tasksLists)
        {
            var layoutViewModel = new LayoutViewModel(tasksLists);
            ActivateItem(layoutViewModel);
            _eventAggregator.Unsubscribe(this);
        }
    }
}