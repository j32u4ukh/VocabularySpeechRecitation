using PureMVC.Interfaces;
using UnityEngine;
using vts.mvc;

namespace vts
{
    public class InitCommand : SimpleCommand
    {
        public override void execute(INotification notification)
        {
            Utils.log("Init");
            MainActivity main = notification.Body as MainActivity;

            AppFacade.getInstance().registerMediator(mediator: new ScrollWordsMediator(mediator_name: MediatorName.ScrollWords1, 
                                                                                       content: main.content));

            AppFacade.getInstance().registerProxy(proxy: new VocabularyProxy(proxy_name: ProxyName.VocabularyProxy, 
                                                                             language: SystemLanguage.English, 
                                                                             table_number: 1));
        }
    }
}


