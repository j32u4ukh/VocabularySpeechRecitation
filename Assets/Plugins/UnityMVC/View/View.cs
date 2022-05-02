using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class View : IView
    {
        // Key: mediator 名稱, Value: mediator
        /// <summary>Mapping of Mediator names to Mediator instances</summary>
        protected readonly ConcurrentDictionary<string, IMediator> mediators;

        public View()
        {
            mediators = new ConcurrentDictionary<string, IMediator>();
        }

        public virtual void register(IMediator mediator)
        {
            if (mediators.TryAdd(mediator.getName(), mediator))
            {
                // Mediator 感興趣的 Notification 們的名稱
                IEnumerable<string> notifications = mediator.subscribeNotifications();
 
                foreach(string notification in notifications)
                {
                    Facade.getInstance().registerNotificationListener(notification, listener: mediator.onNotificationListener);
                }

                // alert the mediator that it has been registered
                mediator.onRegister();
            }
        }

        public virtual bool isExists(string name)
        {
            return mediators.ContainsKey(name);
        }
        
        public virtual IMediator get(string name)
        {
            return mediators.TryGetValue(name, out IMediator mediator) ? mediator : null;
        }

        public virtual IMediator expulsion(string name)
        {
            if (mediators.TryRemove(name, out IMediator mediator))
            {
                // Mediator 感興趣的 Notification 們的名稱
                IEnumerable<string> notifications = mediator.subscribeNotifications();

                foreach (string notification in notifications)
                {
                    Facade.getInstance().removeNotificationListener(notification, mediator.onNotificationListener);
                }

                mediator.onExpulsion();
            }

            return mediator;
        }
    }
}
