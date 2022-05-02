using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityMVC;

namespace VTS
{
    public class SpeechFragment : Mediator
    {
        public Transform content;
        public GameObject prefab;

        private void Start()
        {
            // 主畫面下方，讀取單字組列表
            //AppFacade.getInstance().registerProxy(new SpeechFragmentProxy(proxy_name: vts.ProxyName.GroupList));
        }

        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[]
            {
                Notification.GroupListLoaded
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.GroupListLoaded:
                    displayGroupList();
                    break;
            }
        }

        void displayGroupList()
        {
            Utils.log();

            //foreach (Transform child in content)
            //{
            //    GameManager.getInstance().destory(child);
            //}

            //SpeechFragmentProxy proxy = AppFacade.getInstance().getProxy(proxy_name: vts.ProxyName.GroupList) as SpeechFragmentProxy;
            //Utils.log($"#group: {proxy.table.getRowNumber()}");
            GameObject obj;
            Button button;

            //foreach (List<string> row in proxy.table.iterTable())
            //{
            //    Utils.log(row.toString());

            //    //obj = GameManager.getInstance().getInstantiate(prefab: prefab, parent: content);
            //    //button = obj.GetComponent<Button>();

            //    //// 按鈕呈現文字，說明當前為哪個單字組(Group)
            //    //obj.GetComponent<Text>().text = row[0];
            //    //Utils.log(row[0]);

            //    //button.onClick.AddListener(() =>
            //    //{
            //    //    //SpeechNorm norm = new SpeechNorm(mediator_name: vts.MediatorName.SpeechActivity,
            //    //    //                                 scroll: scroll.gameObject,
            //    //    //                                 proxy_name: ProxyName.VocabularyProxy,
            //    //    //                                 target: SystemLanguage.English,
            //    //    //                                 describe: SystemLanguage.Chinese,
            //    //    //                                 table_name: "table1");

            //    //    //AppFacade.getInstance().sendNotification(ENotification.InitSpeech, body: norm);
            //    //});
            //}
        }
    }
}
