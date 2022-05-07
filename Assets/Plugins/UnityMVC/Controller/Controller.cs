using System.Collections.Concurrent;
using System.Collections.Generic;

namespace UnityMVC
{
    public class Controller : IController
    {
        /// <summary>Mapping of Notification names to Command Class references</summary>
        protected readonly ConcurrentDictionary<string, ICommander> commands;

        public Controller()
        {
            commands = new ConcurrentDictionary<string, ICommander>();
        }

        public void register(ICommander commnad)
        {
            if (commands.TryAdd(commnad.getName(), commnad))
            {
                // Mediator 感興趣的 Notification 們的名稱
                IEnumerable<string> notifications = commnad.subscribeNotifications();

                foreach (string notification in notifications)
                {
                    Facade.getInstance().registerListener(notification, listener: commnad.onNotificationListener);
                }

                // alert the mediator that it has been registered
                commnad.onRegister();
            }
        }

        public bool isExists(string name)
        {
            return commands.ContainsKey(name);
        }

        public ICommander release(string name)
        {
            if (commands.TryRemove(name, out ICommander command))
            {
                // Mediator 感興趣的 Notification 們的名稱
                IEnumerable<string> notifications = command.subscribeNotifications();

                foreach (string notification in notifications)
                {
                    Facade.getInstance().releaseListener(notification, command.onNotificationListener);
                }

                command.onRelease();
            }

            return command;
        }
    }
}
