using System;
using System.IO;
using Autofac;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Authorization;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Synchronization;

namespace GTasksDesktopClient.Core.Shell
{
    public class ShellViewModel : Conductor<object>, IBusyIndicator
    {
        private const string WindowTitle = "Google Tasks Desktop Client";

        private readonly AuthorizationViewModel _authorizationViewModel;
        private readonly LayoutViewModel _layoutViewModel;

        private bool _isBusy;
        private string _message;
        private readonly BusyScope _busyScope;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public ShellViewModel(
            AuthorizationViewModel authorizationViewModel,
            LayoutViewModel layoutViewModel)
        {
            base.DisplayName = WindowTitle;

            _authorizationViewModel = authorizationViewModel;
            _layoutViewModel = layoutViewModel;

            AuthorizationManager.AuthorizationRequired += ShowAuthorizationView;
            AuthorizationManager.AuthorizationSucceeded += ShowLayout;

            _busyScope = new BusyScope(this);
        }

        private void ShowAuthorizationView(Uri authorizationUrl)
        {
            _busyScope.Release();
            _authorizationViewModel.AuthorizationUrl = authorizationUrl;
            ActivateItem(_authorizationViewModel);
        }

        private void ShowLayout()
        {
            _busyScope.Release();
            ActivateItem(_layoutViewModel);
        }
    }
}