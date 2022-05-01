using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Facade
    {
        private static Facade instance = null;

        // 透過 IProxy 對 數據 進行管理
        /// <summary>References to Model</summary>
        protected Model model;

        // 透過 IMediator 對 UI 進行管理
        /// <summary>References to View</summary>
        protected View view;

        // 透過 ICommand 對 命令/流程 進行管理
        /// <summary>References to Controller</summary>
        protected Controller controller;

        protected readonly ConcurrentDictionary<string, List<Action<INotification>>> notification_listeners;

        public Facade()
        {
            notification_listeners = new ConcurrentDictionary<string, List<Action<INotification>>>();

            initeModel();
            initeController();
            initeView();
        }

        public static Facade getInstance()
        {
            if(instance == null)
            {
                instance = new Facade();
            }

            return instance;
        }

        #region Model
        public virtual void initeModel()
        {
            model = new Model();
        } 
        
        /// <summary>
        /// Register an <c>IProxy</c> with the <c>Model</c> by name.
        /// </summary>
        /// <param name="proxy">the <c>IProxy</c> instance to be registered with the <c>Model</c>.</param>
        public virtual void registerProxy(IProxy proxy)
        {
            model.register(proxy);
        }

        /// <summary>
        /// Retrieve an <c>IProxy</c> from the <c>Model</c> by name.
        /// </summary>
        /// <param name="name">the name of the proxy to be retrieved.</param>
        /// <returns>the <c>IProxy</c> instance previously registered with the given <c>proxyName</c>.</returns>
        public virtual IProxy getProxy(string name)
        {
            return model.get(name);
        }

        /// <summary>
        /// Remove an <c>IProxy</c> from the <c>Model</c> by name.
        /// </summary>
        /// <param name="proxyName">the <c>IProxy</c> to remove from the <c>Model</c>.</param>
        /// <returns>the <c>IProxy</c> that was removed from the <c>Model</c></returns>
        public virtual IProxy removeProxy(string proxyName)
        {
            return model.remove(proxyName);
        }

        /// <summary>
        /// Check if a Proxy is registered
        /// </summary>
        /// <param name="name"></param>
        /// <returns>whether a Proxy is currently registered with the given <c>proxyName</c>.</returns>
        public virtual bool isProxyExists(string name)
        {
            return model.isExists(name);
        }
        #endregion

        #region View
        public virtual void initeView()
        {
            view = new View();
        }

        /// <summary>
        /// Register a <c>IMediator</c> with the <c>View</c>.
        /// </summary>
        /// <param name="mediator">a reference to the <c>IMediator</c></param>
        public virtual void registerMediator(IMediator mediator)
        {
            view.register(mediator);
        }

        /// <summary>
        /// Retrieve an <c>IMediator</c> from the <c>View</c>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>the <c>IMediator</c> previously registered with the given <c>mediatorName</c>.</returns>
        public virtual IMediator getMediator(string name)
        {
            return view.get(name);
        }

        /// <summary>
        /// Remove an <c>IMediator</c> from the <c>View</c>.
        /// </summary>
        /// <param name="name">name of the <c>IMediator</c> to be removed.</param>
        /// <returns>the <c>IMediator</c> that was removed from the <c>View</c></returns>
        public virtual IMediator removeMediator(string name)
        {
            return view.remove(name);
        }

        /// <summary>
        /// Check if a Mediator is registered or not
        /// </summary>
        /// <param name="name"></param>
        /// <returns>whether a Mediator is registered with the given <c>mediatorName</c>.</returns>
        public virtual bool isMediatorExists(string name)
        {
            return view.isExists(name);
        }
        #endregion

        #region Controller
        public virtual void initeController()
        {
            controller = new Controller();
        }

        /// <summary>
        /// Register an <c>ICommand</c> with the <c>Controller</c> by Notification name.
        /// </summary>
        /// <param name="notification_name">the name of the <c>INotification</c> to associate the <c>ICommand</c> with</param>
        /// <param name="func">a reference to the Class of the <c>ICommand</c></param>
        public virtual void registerCommand(string notification_name, System.Func<ICommand> func)
        {
            controller.register(notification_name, func);
        }

        /// <summary>
        /// Remove a previously registered <c>ICommand</c> to <c>INotification</c> mapping from the Controller.
        /// </summary>
        /// <param name="notification_name">the name of the <c>INotification</c> to remove the <c>ICommand</c> mapping for</param>
        public virtual void removeCommand(string notification_name)
        {
            controller.remove(notification_name);
        }

        /// <summary>
        /// Check if a Command is registered for a given Notification 
        /// </summary>
        /// <param name="notification_name"></param>
        /// <returns>whether a Command is currently registered for the given <c>notificationName</c>.</returns>
        public virtual bool isCommandExists(string notification_name)
        {
            return controller.isExists(notification_name);
        }
        #endregion

        #region Notification
        /// <summary>
        /// Create and send an <c>INotification</c>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Keeps us from having to construct new notification 
        ///         instances in our implementation code.
        ///     </para>
        /// </remarks>
        /// <param name="notification_name">the name of the notiification to send</param>
        /// <param name="body">the body of the notification (optional)</param>
        /// <param name="type">type the type of the notification (optional)</param>
        public virtual void sendNotification(string notification_name, object body = null, string header = null)
        {
            // Get a reference to the observers list for this notification name
            if (notification_listeners.TryGetValue(notification_name, out List<Action<INotification>> ref_listeners))
            {
                List<Action<INotification>> listeners = new List<Action<INotification>>(ref_listeners);
                Notification notification = new Notification(notification_name, body, header);

                foreach (Action<INotification> listener in listeners)
                {
                    listener(notification);
                }
            }
        }

        public void registerNotificationListener(string notification_name, Action<INotification> listener)
        {
            if (notification_listeners.TryGetValue(notification_name, out List<Action<INotification>> listeners))
            {
                listeners.Add(listener);
            }
            else
            {
                notification_listeners.TryAdd(notification_name, new List<Action<INotification>> { listener });
            }
        }

        public void removeNotificationListener(string notification_name, Action<INotification> listener)
        {
            if (notification_listeners.TryGetValue(notification_name, out List<Action<INotification>> listeners))
            {
                // 若成功移除監聽器，就再檢查該通知的監聽器數量是否為 0，若是則將該通知移除
                if (listeners.Remove(listener) && listeners.Count == 0)
                {
                    notification_listeners.TryRemove(notification_name, out _);
                }
            }
        }
        #endregion
    }
}
