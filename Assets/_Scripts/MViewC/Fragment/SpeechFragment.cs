using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vts.mvc;

namespace vts
{
    public class SpeechFragment : MonoBehaviour
    {
        public Transform content;
        public GameObject prefab;

        private void Start()
        {
            // 主畫面下方，讀取單字組列表
            AppFacade.getInstance().registerProxy(new SpeechFragmentProxy(proxy_name: vts.ProxyName.GroupList));

            // 主畫面下方，初始頁面
            AppFacade.getInstance().registerMediator(new SpeechFragmentMediator(mediator_name: vts.MediatorName.SpeechFragment,
                                                                                fragment: this));
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }
    }
}
