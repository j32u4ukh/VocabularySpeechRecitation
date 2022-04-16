using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    public abstract class Proxy : PureMVC.Patterns.Proxy.Proxy
    {
        /// <summary>
        /// �����g�غc�禡�A�b�غc�禡�������I�s�������غc�禡�AProxy���u���ѤF�@�Ӧ��Ѻc�y
        /// �i�H�b�غc�禡���q�~���ǤJ���data�ϥΡA�]�i�H�b�غc�禡����l�Ƹ��
        /// </summary>
        public Proxy(string proxyName = null, object data = null) : base(proxyName: NAME, data: data)
        {
            ProxyName = proxyName ?? this.GetType().Name;
        }

        #region �мg PureMVC.Patterns.Proxy.Proxy
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
