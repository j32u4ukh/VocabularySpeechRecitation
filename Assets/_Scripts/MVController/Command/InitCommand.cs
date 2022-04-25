using PureMVC.Interfaces;
using vts.mvc;

namespace vts
{
    public class InitCommand : SimpleCommand
    {
        public override void execute(INotification notification)
        {
            Utils.log("Init");
            MainActivity activity = notification.Body as MainActivity;

            //AppFacade.getInstance().registerMediator(mediator: new ScrollWordsMediator(mediator_name: norm.mediator_name,
            //                                                                           scroll: norm.scroll,
            //                                                                           proxy_name: norm.proxy_name));

            //AppFacade.getInstance().registerProxy(proxy: new VocabularyProxy(proxy_name: norm.proxy_name,
            //                                                                 target: norm.target,
            //                                                                 describe: norm.describe,
            //                                                                 table_name: norm.table_name));
        }
    }
}


