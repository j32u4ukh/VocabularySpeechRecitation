using PureMVC.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    public class InitSpeechCommand : SimpleCommand
    {
        public override void execute(INotification notification)
        {
            SpeechNorm norm = notification.Body as SpeechNorm;

            AppFacade.getInstance().registerMediator(mediator: new ScrollWordsMediator(mediator_name: norm.mediator_name,
                                                                                       scroll: norm.scroll,
                                                                                       proxy_name: norm.proxy_name));

            AppFacade.getInstance().registerProxy(proxy: new VocabularyProxy(proxy_name: norm.proxy_name,
                                                                             target: norm.target,
                                                                             describe: norm.describe,
                                                                             table_name: norm.table_name));
        }
    }
}
