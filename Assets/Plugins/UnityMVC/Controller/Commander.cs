using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Commander : MonoBehaviour, ICommander
    {
        string commander_name = null;

        /// <summary>
        /// 預設在 Awake 就利用物件名稱來對 Commander 做註冊，若需要複用等不希望在 Awake 就註冊的需求，
        /// 可以繼承 Commander 之後，將 Awake 複寫，再自行命名與註冊即可
        /// </summary>
        public virtual void Awake()
        {
            register(name: GetType().Name);
        }

        public virtual void OnDestroy()
        {
            Facade.getInstance().expulsionCommander(commander_name: commander_name);
        }

        public virtual void register(string name)
        {
            commander_name = name;
            Facade.getInstance().registerCommander(this);
        }

        public string getName()
        {
            return commander_name;
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
