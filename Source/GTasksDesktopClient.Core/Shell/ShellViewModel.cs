using System;
using System.IO;
using Autofac;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Authorization;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Lists;
using GTasksDesktopClient.Core.Synchronization;

namespace GTasksDesktopClient.Core.Shell
{
    public class ShellViewModel : Conductor<object>, IBusyScope, IHandle<ListsUpdated>
    {
        private const string WindowTitle = "Google Tasks Desktop Client";

        private readonly IContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<Uri, AuthorizationViewModel> _authorizationViewModelFactory;

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

        public ShellViewModel(
            IContainer container,
            IEventAggregator eventAggregator, 
            Func<Uri, AuthorizationViewModel> authorizationViewModelFactory)
        {
            base.DisplayName = WindowTitle;

            _container = container;
            _eventAggregator = eventAggregator;
            _authorizationViewModelFactory = authorizationViewModelFactory;

            _eventAggregator.Subscribe(this);
            AuthorizationManager.AuthorizationRequired += ShowAuthorizationView;
        }

        private void ShowAuthorizationView(Uri authorizationUrl)
        {
            var authorizationViewModel = _authorizationViewModelFactory(authorizationUrl);
            ActivateItem(authorizationViewModel);
        }

        public void Handle(ListsUpdated message)
        {
            _eventAggregator.Unsubscribe(this);
            ShowLayout();
        }

        private void ShowLayout()
        {
            var layoutViewModel = _container.Resolve<LayoutViewModel>();
            ActivateItem(layoutViewModel);
        }
    }
}