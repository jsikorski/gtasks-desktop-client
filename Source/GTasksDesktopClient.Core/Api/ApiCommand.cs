using System;
using System.Net;
using GTasksDesktopClient.Core.Infrastructure;
using GTasksDesktopClient.Core.Utils;
using Google;

namespace GTasksDesktopClient.Core.Api
{
    public abstract class ApiCommand : ICommand, 
        IHandleException<GoogleApiException>, 
        IHandleException<GoogleApiRequestException>, 
        IHandleException<InvalidOperationException>
    {
        public abstract void Execute();

        public virtual void HandleException(GoogleApiException exception)
        {
            MessageBoxService.ShowError("Wyst�pi� b��d po stronie us�ugi Google.");
        }

        public virtual void HandleException(GoogleApiRequestException exception)
        {
            if (exception.HttpStatusCode == HttpStatusCode.BadRequest)
            {
                MessageBoxService.ShowError("Operacja nie jest dozwolona.");
                return;
            }

            MessageBoxService.ShowError("Wyst�pi� b��d. Serwer odm�wi� wykonania operacji.");
        }

        public virtual void HandleException(InvalidOperationException exception)
        {
            MessageBoxService.ShowError("Operacja nie jest dozwolona.");
        }
    }
}