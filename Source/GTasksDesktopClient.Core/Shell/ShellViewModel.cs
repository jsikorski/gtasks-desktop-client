using System;
using System.Diagnostics;
using Caliburn.Micro;
using GApiHelpers.Authorization;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Infrastructure.BackgroundTasks;
using GTasksDesktopClient.Core.Layout;
using GTasksDesktopClient.Core.Utils;

namespace GTasksDesktopClient.Core.Shell
{
    public class ShellViewModel : Conductor<object>, IBusyIndicator
    {
        private const string WindowTitle = "Google Tasks Desktop Client";

        private readonly LayoutViewModel _layoutViewModel;
        private readonly AuthorizationManager _authorizationManager;
        private readonly BackgroundTasksManager _backgroundTasksManager;

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
            AuthorizationManager authorizationManager, 
            BackgroundTasksManager backgroundTasksManager)
        {
            base.DisplayName = WindowTitle;
            IsBusy = true;

            _layoutViewModel = layoutViewModel;
            _authorizationManager = authorizationManager;
            _backgroundTasksManager = backgroundTasksManager;

            _authorizationManager.AuthorizationRequired += ShowAuthorizationView;
            _authorizationManager.AuthorizationSucceeded += ShowLayout;
            _authorizationManager.AuthorizationCanceled += ShowAuthorizationCanceledMessage;
            _authorizationManager.AuthorizationNotSupported += ShowAuthorizationNotSupportedError;
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
            _backgroundTasksManager.StopAll();

            MessageBoxService.ShowInfo("Wyra�enie zgody na " +
                                       "dost�p do zasob�w konta Google " +
                                       "jest niezb�dne dla prawid�owego " +
                                       "dzia�nia aplikacji.");
            TryClose();
        }

        private void ShowAuthorizationNotSupportedError()
        {
            _backgroundTasksManager.StopAll();
            
            MessageBoxService.ShowError("Autoryzacja zwi�zana z dost�pem do " +
                                        "konta Google nie mo�e by� zrealizowana. " +
                                        "Sprawd� ustawienia zapory sieciowej " +
                                        "i spr�buj ponownie.");
            TryClose();
        }
    }
}