using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Mediator : MonoBehaviour, IMediator
    {
        string mediator_name = null;

        public virtual void Awake()
        {
            mediator_name = GetType().ToString();
            Facade.getInstance().registerMediator(this);
        }

        public virtual void onRegister()
        {

        }

        public virtual void onExpulsion()
        {

        }

        public void setName(string name)
        {
            mediator_name = name;
        }

        public string getName()
        {
            return mediator_name;
        }

        public virtual IEnumerable<string> subscribeNotifications()
        {
            return Facade.No_TIFICATION;
        }

        public virtual void onNotificationListener(INotification notification)
        {
            
        }
    }
}
