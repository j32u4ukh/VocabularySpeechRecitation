using PureMVC.Interfaces;
using UnityEngine;
using vts.mvc;

namespace vts
{
    public class SpeakCommand : SimpleCommand
    {
        public override void execute(INotification notification)
        {
            Utils.log();
            VocabularyNorm vocab = notification.Body as VocabularyNorm;
            SpeechManager.getInstance().startReciteContent(vocab: vocab, 
                                                           target: SystemLanguage.English, 
                                                           describe: SystemLanguage.ChineseTraditional);
        }
    }
}
