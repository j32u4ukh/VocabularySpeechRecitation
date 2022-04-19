using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vts.mvc;

namespace vts
{
    /// <summary>
    /// Activity，持有 UI 的各種控制元件，提供一些更新顯示等方法供外界呼叫，屬於 MVC 的 V。
    /// 注意：Activity 類繼承 MonoBehaviour 類，是掛載在皮膚上的指令碼；PureMVC 中也有一個 View 類，這個類繼承自 IView 介面，
    /// 使用框架時不會涉及到 View 類；這裡的 Activity 和 View 類並不相同。
    /// </summary>
    public class MainActivity : MonoBehaviour
    {
        public Transform content;

        // Start is called before the first frame update
        void Start()
        {
            AppFacade.getInstance().init(this);
        }
    }
}
