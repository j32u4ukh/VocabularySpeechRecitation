using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public interface INotify
    {
        IEnumerable<string> subscribeNotifications();

        void onNotificationListener(INotification notification);
    }
}
