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

        /// <summary>
        /// 直接繼承 Proxy 並使用 base() 建構子的類別可以適用，因為已知使用類別名稱作為註冊時的名稱，
        /// 返回時也已經完成類別的轉換。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxy_name"></param>
        /// <returns></returns>
        public virtual bool tryGetProxy<T>(out T t_proxy, string proxy_name = null) where T : class
        {
            if (string.IsNullOrEmpty(proxy_name))
            {
                proxy_name = typeof(T).Name;
            }

            bool is_exists = tryGetProxy(proxy_name: proxy_name, out IProxy proxy);

            if (is_exists)
            {
                t_proxy = proxy as T;
            }
            else
            {
                t_proxy = null;
            }

            return is_exists;
        }

        /// <summary>
        /// 繼承 Proxy 並做了覆寫的類別可以適用，同一個 Proxy 利用不同名稱進行註冊，使用該名稱來取得，並須自行進行轉型。
        /// </summary>
        /// <param name="proxy_name"></param>
        /// <returns></returns>
        public virtual bool tryGetProxy(string proxy_name, out IProxy proxy)
        {
            return model.tryGet(proxy_name, out proxy);
        }

        public virtual IProxy releaseProxy(string proxy_name)
        {
            return model.release(proxy_name);
        }

        public virtual bool isProxyExists<T>(string proxy_name = null) where T : class
        {
            if (string.IsNullOrEmpty(proxy_name))
            {
                proxy_name = typeof(T).Name;
            }

            return isProxyExists(proxy_name);
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

        public virtual IMediator releaseMediator(string mediator_name)
        {
            return view.release(mediator_name);
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

        public virtual void releaseCommander(string commander_name)
        {
            controller.release(commander_name);
        }

        public virtual bool isCommanderExists(string commander_name)
        {
            return controller.isExists(commander_name);
        }

        //public virtual bool tryGetCommander<T>(out T commander, string commander_name = null) where T : class
        //{

        //}

        //public virtual bool tryGetCommander(string commander_name, out ICommander commander)
        //{

        //}
        #endregion

        #region Notification
        public virtual void sendNotification(string notification_name, object data = null)
        {
            // Get a reference to the observers list for this notification name
            if (notification_listeners.TryGetValue(notification_name, out List<Action<INotification>> ref_listeners))
            {
                List<Action<INotification>> listeners = new List<Action<INotification>>(ref_listeners);
                Notification notification = new Notification(notification_name, data);

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

        public virtual void releaseListener(string notification_name, Action<INotification> listener)
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
