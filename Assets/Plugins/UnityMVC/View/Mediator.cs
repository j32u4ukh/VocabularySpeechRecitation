using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Mediator : MonoBehaviour, IMediator
    {
        string mediator_name = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void onRegister()
        {

        }

        public virtual void onRemove()
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

        public virtual IEnumerable<string> listenToNotifications()
        {
            return new string[0];
        }

        public virtual void onNotificationListener(INotification notification)
        {
            
        }
    }
}
