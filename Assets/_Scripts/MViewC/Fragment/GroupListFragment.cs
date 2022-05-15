using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityMVC;

namespace VTS
{
    public class GroupListFragment : Mediator
    {
        public Transform content;
        public GameObject prefab;

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

            if(Facade.getInstance().tryGetProxy(out GroupListProxy group_list))
            {
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
                        Facade.getInstance().sendNotification(Notification.OpenSpeechActivity, data: row[1]);
                    });
                }

                // TODO: 根據卡片數量，設置 content 高度
            }
        }
    }
}
