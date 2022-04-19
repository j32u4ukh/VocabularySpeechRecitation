using PureMVC.Interfaces;
using UnityEngine;

namespace vts.mvc
{
    public class SpeakCommand : SimpleCommand
    {
        public override void execute(INotification notification)
        {
            Utils.log();
            // Proxy name
            // vocabulary
            // description
            // taregt language
            // describe language
            VocabularyNorm vocab = notification.Body as VocabularyNorm;
            SpeechManager.getInstance().startReciteContent(vocab: vocab, 
                                                           target: SystemLanguage.English, 
                                                           describe: SystemLanguage.ChineseTraditional);
        }
    }
}
