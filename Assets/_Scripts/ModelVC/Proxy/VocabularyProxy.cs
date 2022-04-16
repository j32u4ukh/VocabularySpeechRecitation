using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vts.mvc;

namespace vts
{
    public class VocabularyProxy : Proxy
    {
        /// <summary>
        /// 必須寫建構函式，在建構函式中必須呼叫父類的建構函式，Proxy中只提供了一個有參構造
        /// 可以在建構函式中從外部傳入資料data使用，也可以在建構函式中初始化資料
        /// </summary>
        public VocabularyProxy()
        {
            Debug.Log($"ProxyName: {ProxyName}");
        }

        public override void onRegister()
        {

        }

        public override void onRemove()
        {

        }
    }
}

