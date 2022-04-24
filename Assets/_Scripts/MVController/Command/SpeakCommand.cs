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

            // ���w���w�����e
            SpeechManager.getInstance().startReciteContent(vocab: vocab,
                                                           target: SystemLanguage.English,
                                                           describe: SystemLanguage.ChineseTraditional,
                                                           modes: Config.modes,
                                                           callback: finishedReadingCallback);
        }

        void finishedReadingCallback()
        {
            // �����᧹�A�e�X�q�� ENotification.FinishedReading
            AppFacade.getInstance().sendNotification(ENotification.FinishedReading, body: norm);
        }
    }
}
