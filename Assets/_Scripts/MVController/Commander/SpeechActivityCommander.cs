using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class SpeechActivityCommander : Commander
    {
        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[] {
                Notification.InitSpeechActivity
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.InitSpeechActivity:
                    (int, int) data = notification.getData<int, int>();

                    if (data.Item1.Equals(1))
                    {
                        Utils.log(Notification.InitSpeechActivity);
                        Utils.log($"value: {data.Item2}");
                    }
                    break;
            }
        }

        void init()
        {
            //SpeechNorm norm = new SpeechNorm(mediator_name: vts.MediatorName.SpeechActivity,
            //                                 scroll: scroll,
            //                                 proxy_name: ProxyName.VocabularyProxy,
            //                                 target: Config.target,
            //                                 describe: Config.describe,
            //                                 table_name: "table1");

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
