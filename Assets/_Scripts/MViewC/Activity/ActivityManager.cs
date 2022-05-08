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
                Notification.OpenMainActivity,
                Notification.OpenSpeechActivity,
                Notification.Speak
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.OpenMainActivity:
                    // 開啟 MainActivity
                    openMainActivity();
                    break;

                case Notification.OpenSpeechActivity:
                    // 開啟 SpeechActivity，並根據單字組的名稱，初始化 GroupProxy
                    initSpeechActivity(source: notification.getData<string>());
                    break;

                case Notification.Speak:
                    VocabularyNorm norm = notification.getData<VocabularyNorm>();
                    Utils.log($"norm: {norm}");
                    speak(norm: norm);
                    break;
            }
        }

        void openMainActivity()
        {
            Utils.log();
            current.SetActive(false);
            current = main;
            current.SetActive(true);
        }

        void initSpeechActivity(string source)
        {
            current.SetActive(false);
            current = speech;
            current.SetActive(true);

            // GroupProxy 尚未存在
            if (!Facade.getInstance().tryGetProxy(out GroupProxy proxy))
            {
                Utils.log("首次建構 GroupProxy");
                new GroupProxy(source: source);
            }

            // GroupProxy 已存在，但要以新的數據源來載入
            else if (!proxy.getSource().Equals(source))
            {
                Utils.log($"GroupProxy 已存在({proxy.getSource()})，但要以新的數據源({source})來載入");
                proxy.load(source: source);
            }
            else
            {
                Utils.log("GroupProxy 已存在，且使用相同數據源");
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
    }
}
