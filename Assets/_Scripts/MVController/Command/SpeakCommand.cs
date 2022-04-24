using PureMVC.Interfaces;
using UnityEngine;

namespace vts.mvc
{
    public class SpeakCommand : SimpleCommand
    {
        SpeakNorm norm;

        public override void execute(INotification notification)
        {
            Utils.log();
            norm = notification.Body as SpeakNorm;
            VocabularyProxy proxy = AppFacade.getInstance().getProxy(proxy_name: norm.proxy_name) as VocabularyProxy;
            VocabularyNorm vocab = proxy.getVocabulary(index: norm.index);

            // 念誦指定的內容
            SpeechManager.getInstance().startReciteContent(vocab: vocab,
                                                           target: SystemLanguage.English,
                                                           describe: SystemLanguage.ChineseTraditional,
                                                           modes: Config.modes,
                                                           callback: finishedReadingCallback);
        }

        void finishedReadingCallback()
        {
            // 全部唸完，送出通知 ENotification.FinishedReading
            AppFacade.getInstance().sendNotification(ENotification.FinishedReading, body: norm);
        }
    }
}
