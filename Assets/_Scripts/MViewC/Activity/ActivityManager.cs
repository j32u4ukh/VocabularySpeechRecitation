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
                Notification.InitSpeechActivity
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.InitSpeechActivity:
                    (int, int) data = notification.getData<int, int>();

                    if (data.Item1.Equals(0))
                    {
                        initSpeechActivity(value: data.Item2);
                    }
                    break;
            }
        }

        void initSpeechActivity(int value)
        {
            current.SetActive(false);
            current = speech;
            current.SetActive(true);

            Facade.getInstance().sendNotification(Notification.InitSpeechActivity, data: (1, value));
        }
    }
}
