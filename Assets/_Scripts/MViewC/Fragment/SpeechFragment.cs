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

            foreach (Transform child in content)
            {
                Destroy(child);
            }

            GroupListProxy group_list = Facade.getInstance().getProxy<GroupListProxy>();
            Utils.log($"#group: {group_list.getRowNumber()}");
            GameObject obj;
            Button button;

            foreach (List<string> row in group_list.iterTable())
            {
                obj = Instantiate(original: prefab, parent: content);
                button = obj.GetComponent<Button>();

                // 按鈕呈現文字，說明當前為哪個單字組(Group)
                obj.GetComponentInChildren<Text>().text = row[0];

                button.onClick.AddListener(() =>
                {
                    //speechnorm norm = new speechnorm(mediator_name: vts.mediatorname.speechactivity,
                    //                                 scroll: scroll.gameobject,
                    //                                 proxy_name: proxyname.vocabularyproxy,
                    //                                 target: systemlanguage.english,
                    //                                 describe: systemlanguage.chinese,
                    //                                 table_name: "table1");

                    //appfacade.getinstance().sendnotification(enotification.initspeech, body: norm);
                    Facade.getInstance().sendNotification(Notification.InitSpeechActivity);
                });
            }
        }
    }
}
