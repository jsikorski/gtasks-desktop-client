using System;
using System.IO;
using Autofac;
using Caliburn.Micro;
using GTasksDesktopClient.Core.Authorization;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Lists;

namespace GTasksDesktopClient.Core.Shell
{
    public class ShellViewModel : Conductor<object>
    {
        private const string WindowTitle = "Google Tasks Desktop Client";
        
        private readonly IContainer _container;

        public ShellViewModel(IContainer container)
        {
            base.DisplayName = WindowTitle;

            _container = container;
            AuthorizationManager.AuthorizationRequired += ShowAuthorizationView;
        }

        protected override void OnInitialize()
        {
            ShowLayout();
        }

        private void ShowAuthorizationView(Uri authorizationUrl)
        {
            var authorizationViewModel = _container.Resolve<AuthorizationViewModel>(new TypedParameter(typeof(Uri), authorizationUrl));
            ActivateItem(authorizationViewModel);
        }

        private void ShowLayout()
        {
            var getAllLists = _container.Resolve<GetAllLists>();
            CommandsInvoker.ExecuteCommand(getAllLists);
        }
    }
}