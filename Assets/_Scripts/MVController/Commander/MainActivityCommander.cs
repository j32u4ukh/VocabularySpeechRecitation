using System.Collections.Generic;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class MainActivityCommander : Commander
    {
        private void Update()
        {
            // 對應手機上的返回鍵
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

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
            string source;

            switch (tag)
            {
                // English -> Chinese
                case 1006:
                default:
                    source = "EnTw";
                    break;
            }

            Utils.log($"tag: {tag}, source: {source}");

            // GroupListProxy 尚未存在
            if (!Facade.getInstance().tryGetProxy(out GroupListProxy proxy))
            {
                // 建構 GroupListProxy 物件的同時，也向 Facade 註冊了，因此離開此區塊後仍可存取 GroupListProxy
                new GroupListProxy(source: source);
                Utils.log("首次建構 GroupListProxy");
            }

            // GroupListProxy 已存在，但要以新的數據源來載入
            else if (!proxy.getSource().Equals(source))
            {
                Utils.log($"GroupListProxy 已存在({proxy.getSource()})，但要以新的數據源({source})來載入");
                proxy.load(source: source);
            }
            else
            {
                Utils.log("GroupListProxy 已存在，且使用相同數據源");
            }
        }
    }
}