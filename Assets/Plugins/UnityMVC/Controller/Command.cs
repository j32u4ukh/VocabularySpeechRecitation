using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Command : MonoBehaviour, ICommand
    {
        string command_name = null;

        public virtual void Awake()
        {
            command_name = GetType().ToString();
            Facade.getInstance().registerCommand(this);
        }

        public void setName(string name)
        {
            command_name = name;
        }

        public string getName()
        {
            return command_name;
        }

        public virtual void onRegister()
        {

        }

        public virtual IEnumerable<string> subscribeNotifications()
        {
            return Facade.No_TIFICATION;
        }

        public virtual void onNotificationListener(INotification notification)
        {

        }

        public virtual void onExpulsion()
        {

        }
    }
}
