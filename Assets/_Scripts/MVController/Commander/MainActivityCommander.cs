using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class MainActivityCommander : Commander
    {
        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[] { 
                Notification.InitGroupList
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.InitGroupList:
                    initGroupList();
                    break;
            }
        }

        void initGroupList()
        {
            int tag = 100 * (int)Config.target + (int)Config.describe;
            string file_name;

            switch (tag)
            {
                // English -> Chinese
                case 1006:
                default:
                    file_name = "EnTw";
                    break;
            }

            string path = Path.Combine(Application.streamingAssetsPath, "vocabulary", $"{file_name}.csv");
            Utils.log($"tag: {tag}, file_name: {file_name}");

            // 建構 GroupListProxy 物件的同時，也向 Facade 註冊了，因此離開此區塊後仍可存取 GroupListProxy
            GroupListProxy group_list = new GroupListProxy(path: path);
        }
    }
}