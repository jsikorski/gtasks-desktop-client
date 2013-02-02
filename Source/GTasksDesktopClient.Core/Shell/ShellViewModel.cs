using System;
using System.Diagnostics;
using Caliburn.Micro;
using GApiHelpers.Authorization;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;

namespace GTasksDesktopClient.Core.Shell
{
    public class ShellViewModel : Conductor<object>, IBusyIndicator
    {
        private const string WindowTitle = "Google Tasks Desktop Client";

        private readonly LayoutViewModel _layoutViewModel;
        private readonly AuthorizationManager _authorizationManager;

        private bool _isBusy;
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
            LayoutViewModel layoutViewModel, 
            AuthorizationManager authorizationManager)
        {
            base.DisplayName = WindowTitle;
            IsBusy = true;

            _layoutViewModel = layoutViewModel;
            _authorizationManager = authorizationManager;

            _authorizationManager.AuthorizationRequired += ShowAuthorizationView;
            _authorizationManager.AuthorizationSucceeded += ShowLayout;
        }

        private void ShowAuthorizationView(Uri authorizationUrl)
        {
            Process.Start(authorizationUrl.ToString());
        }

        private void ShowLayout()
        {
            ActivateItem(_layoutViewModel);
        }
    }
}