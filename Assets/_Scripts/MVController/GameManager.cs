using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class GameManager : Commander
    {
        [SerializeField] private GameObject reporter;

        private void Start()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            reporter.SetActive(true);
#else
            reporter.SetActive(false);
#endif
        }

        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[] {
                Notification.Speak
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
            }
        }

        public void speak(VocabularyNorm norm)
        {
            Utils.log($"vocabulary: {norm.getVocabulary()}, description: {norm.getDescription()}");

            // 念誦指定的內容
            SpeechManager.getInstance()
                         .startReciteContent(vocabulary: norm.getVocabulary(),
                                             description: norm.getDescription(),
                                             target: Config.target,
                                             describe: Config.describe,
                                             modes: Config.modes,
                                             callback: () =>
                                             {
                                                 Utils.log($"sendNotification: {Notification.FinishedReading}");

                                                 // 全部唸完，送出通知 ENotification.FinishedReading
                                                 Facade.getInstance().sendNotification(Notification.FinishedReading);
                                             });
        }
    }
}
