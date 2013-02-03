using System;
using System.Diagnostics;
using System.Windows;
using Caliburn.Micro;
using GApiHelpers.Authorization;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Utils;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

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
            _authorizationManager.AuthorizationCanceled += ShowAuthorizationCanceledMessage;
        }

        private void ShowAuthorizationView(Uri authorizationUrl)
        {
            Process.Start(authorizationUrl.ToString());
        }

        private void ShowLayout()
        {
            ActivateItem(_layoutViewModel);
        }

        private void ShowAuthorizationCanceledMessage()
        {
            MessageBoxService.ShowInfo("Wyra¿enie zgody na " +
                                       "dostêp do zasobów konta Google " +
                                       "jest niezbêdne dla prawid³owego " +
                                       "dzia³nia aplikacji. Aby to zrobiæ " +
                                       "uruchom ponownie aplikacjê.");
            TryClose();
        }
    }
}