using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityMVC
{
    public abstract class Proxy : IProxy
    {
        string proxy_name;

        /// <summary>
        /// 預設在 Awake 就利用物件名稱來對 Proxy 做註冊，若需要複用等不希望在 Awake 就註冊的需求，
        /// 可以繼承 Proxy 之後，將 Awake 複寫，再自行命名與註冊即可
        /// </summary>
        public Proxy()
        {
            register(name: GetType().Name);
        }

        public virtual void register(string name)
        {
            proxy_name = name;
            Facade.getInstance().registerProxy(proxy: this);
        }

        public string getName()
        {
            return proxy_name;
        }

        public abstract void release();

        #region Life cycle
        public virtual void onRegister()
        {

        }

        public virtual void onExpulsion()
        {

        } 
        #endregion
    }
}
