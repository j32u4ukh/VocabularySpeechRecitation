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
                Notification.Speak,
                Notification.InitSpeechActivity
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.InitSpeechActivity:
                    Utils.log(Notification.InitSpeechActivity);
                    break;
                case Notification.Speak:
                    SpeakNorm norm = notification.getData() as SpeakNorm;
                    speak(norm: norm);
                    break;
            }
        }

        public void speak(SpeakNorm norm)
        {
            Utils.log();
            VocabularyProxy proxy = Facade.getInstance().getProxy(proxy_name: norm.proxy_name) as VocabularyProxy;
            VocabularyNorm vocab = proxy.getVocabulary(index: norm.index);

            // 念誦指定的內容
            SpeechManager.getInstance()
                         .startReciteContent(vocab: vocab,
                                             target: SystemLanguage.English,
                                             describe: SystemLanguage.ChineseTraditional,
                                             modes: Config.modes,
                                             callback: () =>
                                             {
                                                 // 全部唸完，送出通知 ENotification.FinishedReading
                                                 Facade.getInstance().sendNotification(Notification.FinishedReading, data: norm);
                                             });
        }
    }
}
