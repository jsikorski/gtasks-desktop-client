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
    public class ShellViewModel : Conductor<object>, IBusyScope
    {
        private const string WindowTitle = "Google Tasks Desktop Client";

        private readonly IContainer _container;
        private readonly Func<Uri, AuthorizationViewModel> _authorizationViewModelFactory;

        private bool _isBusy;
        private string _message;
        private BusyScopeContext _busyScopeContext;

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
            Func<Uri, AuthorizationViewModel> authorizationViewModelFactory)
        {
            base.DisplayName = WindowTitle;

            _container = container;
            _authorizationViewModelFactory = authorizationViewModelFactory;

            AuthorizationManager.AuthorizationRequired += ShowAuthorizationView;
            AuthorizationManager.AuthorizationSucceeded += ShowLayout;

            _busyScopeContext = new BusyScopeContext(this);
        }

        private void ShowAuthorizationView(Uri authorizationUrl)
        {
            _busyScopeContext.Dispose();

            var authorizationViewModel = _authorizationViewModelFactory(authorizationUrl);
            ActivateItem(authorizationViewModel);
        }

        private void ShowLayout()
        {
            _busyScopeContext.Dispose();

            var layoutViewModel = _container.Resolve<LayoutViewModel>();
            ActivateItem(layoutViewModel);
        }
    }
}