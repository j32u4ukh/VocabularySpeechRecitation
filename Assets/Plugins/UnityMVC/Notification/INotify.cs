using System.Collections.Generic;

namespace UnityMVC
{
    public interface INotify
    {
        IEnumerable<string> subscribeNotifications();

        void onNotificationListener(INotification notification);
    }
}
