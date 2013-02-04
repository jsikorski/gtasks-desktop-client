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
            MessageBoxService.ShowError("Wyst¹pi³ b³¹d po stronie us³ugi Google.");
        }

        public virtual void HandleException(GoogleApiRequestException exception)
        {
            if (exception.HttpStatusCode == HttpStatusCode.BadRequest)
            {
                MessageBoxService.ShowError("Operacja nie jest dozwolona.");
                return;
            }

            MessageBoxService.ShowError("Wyst¹pi³ b³¹d. Serwer odmówi³ wykonania operacji.");
        }

        public virtual void HandleException(InvalidOperationException exception)
        {
            MessageBoxService.ShowError("Operacja nie jest dozwolona.");
        }
    }
}