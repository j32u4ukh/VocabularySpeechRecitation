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
    public class SpeechActivity : MonoBehaviour
    {
        public GameObject scroll;
        public GameObject card_prefab;

#if DEBUG
        public Vector2 left;
        public Vector2 right;
#endif

        // Start is called before the first frame update
        void Start()
        {
            AppFacade.getInstance().registerCommand(ENotification.InitSpeech, () =>
            {
                return new InitSpeechCommand();
            });

            SpeechNorm norm = new SpeechNorm(mediator_name: MediatorName.ScrollWords1,
                                             scroll: scroll,
                                             proxy_name: ProxyName.VocabularyProxy,
                                             target: SystemLanguage.English,
                                             describe: SystemLanguage.Chinese,
                                             table_name: "table1");

            AppFacade.getInstance().sendNotification(ENotification.InitSpeech, body: norm);
        }

#if DEBUG
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            left = new Vector2(0, scroll.transform.position.y);
            right = new Vector2(1080, scroll.transform.position.y);
            Gizmos.DrawLine(left, right);
        }
#endif
    }
}
