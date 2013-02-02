using System;
using System.Diagnostics;
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

        private readonly LayoutViewModel _layoutViewModel;

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

        public ShellViewModel(LayoutViewModel layoutViewModel)
        {
            base.DisplayName = WindowTitle;
            IsBusy = true;

            _layoutViewModel = layoutViewModel;

            AuthorizationManager.AuthorizationRequired += ShowAuthorizationView;
            AuthorizationManager.AuthorizationSucceeded += ShowLayout;
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