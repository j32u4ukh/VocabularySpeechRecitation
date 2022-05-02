using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Controller : IController
    {
        /// <summary>Mapping of Notification names to Command Class references</summary>
        protected readonly ConcurrentDictionary<string, ICommand> commands;

        public Controller()
        {
            commands = new ConcurrentDictionary<string, ICommand>();
        }

        public void register(ICommand commnad)
        {
            if (commands.TryAdd(commnad.getName(), commnad))
            {
                // Mediator 感興趣的 Notification 們的名稱
                IEnumerable<string> notifications = commnad.subscribeNotifications();

                foreach (string notification in notifications)
                {
                    Facade.getInstance().registerNotificationListener(notification, listener: commnad.onNotificationListener);
                }

                // alert the mediator that it has been registered
                commnad.onRegister();
            }
        }

        public bool isExists(string name)
        {
            return commands.ContainsKey(name);
        }

        public ICommand expulsion(string name)
        {
            if (commands.TryRemove(name, out ICommand command))
            {
                // Mediator 感興趣的 Notification 們的名稱
                IEnumerable<string> notifications = command.subscribeNotifications();

                foreach (string notification in notifications)
                {
                    Facade.getInstance().removeNotificationListener(notification, command.onNotificationListener);
                }

                command.onExpulsion();
            }

            return command;
        }
    }
}
