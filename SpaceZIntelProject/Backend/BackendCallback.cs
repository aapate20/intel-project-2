using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Backend
{
    // Callback Interface for two way communication between connected clients.
    public interface IBackendCallback
    {
        [OperationContract]
        void ReceiveCommand(string command);
    }
}