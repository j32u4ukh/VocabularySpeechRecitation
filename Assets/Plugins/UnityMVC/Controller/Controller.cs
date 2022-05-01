using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Controller : IController
    {
        /// <summary>Mapping of Notification names to Command Class references</summary>
        protected readonly System.Collections.Concurrent.ConcurrentDictionary<string, Func<ICommand>> commands;

        public void register(string notification_name, Func<ICommand> func)
        {
            if (commands.TryGetValue(notification_name, out _) == false)
            {
                Facade.getInstance().registerNotificationListener(notification_name: notification_name, 
                                                                  listener: execute);
            }

            commands[notification_name] = func;
        }

        public bool isExists(string name)
        {
            throw new NotImplementedException();
        }

        public void execute(INotification notification)
        {
            if (commands.TryGetValue(notification.getName(), out Func<ICommand> func))
            {
                ICommand command = func();
                command.execute(notification);
            }
        }

        public void remove(string notification_name)
        {
            if (commands.TryRemove(notification_name, out _))
            {
                Facade.getInstance().removeNotificationListener(notification_name, execute);
            }
        }
    }
}
