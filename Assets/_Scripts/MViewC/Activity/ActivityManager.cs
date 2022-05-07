using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class ActivityManager : Mediator
    {
        [SerializeField] GameObject main;
        [SerializeField] GameObject speech;

        GameObject current;

        private void Start()
        {
            // 預設開啟時的 Activity 為 MainActivity
            current = main;
        }

        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[] {
                Notification.OpenSpeechActivity
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.OpenSpeechActivity:
                    // 開啟 SpeechActivity，並根據單字組的名稱，初始化 GroupProxy
                    initSpeechActivity(source: notification.getData<string>());
                    break;
            }
        }

        void initSpeechActivity(string source)
        {
            current.SetActive(false);
            current = speech;
            current.SetActive(true);

            GroupProxy proxy = new GroupProxy(source: source);
        }
    }
}
