using System;

namespace GTasksDesktopClient.Core.Infrastructure
{
    public interface IHandleException<in T> where T : Exception
    {
       void HandleException(T exception);
    }
}