using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public interface IMediator : IRegister
    {
        IEnumerable<string> listenToNotifications();

        void onNotificationListener(INotification notification);
    }
}
