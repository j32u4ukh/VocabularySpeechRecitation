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
        protected Model model;

        // 透過 IMediator 對 UI 進行管理
        protected View view;

        // 透過 ICommander 對 命令/流程 進行管理
        protected Controller controller;

        protected readonly ConcurrentDictionary<string, List<Action<INotification>>> notification_listeners;

        public static string[] No_TIFICATION = new string[0];

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
        
        public virtual void registerProxy(IProxy proxy)
        {
            model.register(proxy);
        }

        public virtual T getProxy<T>(string proxy_name = null) where T : class
        {
            if (string.IsNullOrEmpty(proxy_name))
            {
                proxy_name = typeof(T).Name;
            }

            return getProxy(proxy_name: proxy_name) as T;
        }

        public virtual IProxy getProxy(string proxy_name)
        {
            return model.get(proxy_name);
        }

        public virtual IProxy expulsionProxy(string proxy_name)
        {
            return model.expulsion(proxy_name);
        }

        public virtual bool isProxyExists(string proxy_name)
        {
            return model.isExists(proxy_name);
        }
        #endregion

        #region View
        public virtual void initeView()
        {
            view = new View();
        }

        public virtual void registerMediator(IMediator mediator)
        {
            view.register(mediator);
        }

        public virtual IMediator getMediator(string mediator_name)
        {
            return view.get(mediator_name);
        }

        public virtual IMediator expulsionMediator(string mediator_name)
        {
            return view.expulsion(mediator_name);
        }

        public virtual bool isMediatorExists(string mediator_name)
        {
            return view.isExists(mediator_name);
        }
        #endregion

        #region Controller
        public virtual void initeController()
        {
            controller = new Controller();
        }

        public virtual void registerCommander(ICommander commander)
        {
            controller.register(commander);
        }

        public virtual void expulsionCommander(string commander_name)
        {
            controller.expulsion(commander_name);
        }


        public virtual bool isCommanderExists(string commander_name)
        {
            return controller.isExists(commander_name);
        }
        #endregion

        #region Notification
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

        public virtual void registerListener(string notification_name, Action<INotification> listener)
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

        public virtual void removeListener(string notification_name, Action<INotification> listener)
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
