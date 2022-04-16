using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    public abstract class Proxy : PureMVC.Patterns.Proxy.Proxy
    {
        /// <summary>
        /// 必須寫建構函式，在建構函式中必須呼叫父類的建構函式，Proxy中只提供了一個有參構造
        /// 可以在建構函式中從外部傳入資料data使用，也可以在建構函式中初始化資料
        /// </summary>
        public Proxy(string proxyName = null, object data = null) : base(proxyName: NAME, data: data)
        {
            ProxyName = proxyName ?? this.GetType().Name;
        }

        #region 覆寫 PureMVC.Patterns.Proxy.Proxy
        /// <summary>
        /// Called by the Model when the Proxy is registered
        /// </summary>
        public override void OnRegister()
        {
            onRegister();
        }

        /// <summary>
        /// Called by the Model when the Proxy is removed
        /// </summary>
        public override void OnRemove()
        {
            onRemove();
        }
        #endregion

        /// <summary>
        /// Called by the Model when the Proxy is registered
        /// </summary>
        public abstract void onRegister();

        /// <summary>
        /// Called by the Model when the Proxy is removed
        /// </summary>
        public abstract void onRemove();
    }
}
