using System.Collections.Generic;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class ActivityManager : Mediator
    {
        [SerializeField] private GameObject main;
        [SerializeField] private GameObject speech;
        [SerializeField] private GameObject reporter;

        private GameObject current;

        private void Start()
        {
            // 預設開啟時的 Activity 為 MainActivity
            current = main;


#if !UNITY_EDITOR && UNITY_ANDROID
            reporter.SetActive(true);
#else
            reporter.SetActive(false);
#endif
        }

        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[] {
                Notification.Speak,
                Notification.OpenSpeechActivity
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.Speak:
                    VocabularyNorm norm = notification.getData<VocabularyNorm>();
                    Utils.log($"norm: {norm}");
                    speak(norm: norm);
                    break;

                case Notification.OpenSpeechActivity:
                    // 開啟 SpeechActivity，並根據單字組的名稱，初始化 GroupProxy
                    initSpeechActivity(source: notification.getData<string>());
                    break;
            }
        }

        public void speak(VocabularyNorm norm)
        {
            // 念誦指定的內容
            SpeechManager.getInstance()
                         .startReciteContent(vocabulary: norm.getVocabulary(),
                                             description: norm.getDescription(),
                                             target: Config.target,
                                             describe: Config.describe,
                                             modes: Config.modes,
                                             callback: () =>
                                             {
                                                 // 全部唸完，送出通知 ENotification.FinishedReading
                                                 Facade.getInstance().sendNotification(Notification.FinishedReading);
                                             });
        }

        void initSpeechActivity(string source)
        {
            current.SetActive(false);
            current = speech;
            current.SetActive(true);

            new GroupProxy(source: source);
        }
    }
}
