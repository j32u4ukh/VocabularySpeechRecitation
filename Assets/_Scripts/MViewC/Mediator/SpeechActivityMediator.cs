using PureMVC.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    public class SpeechActivityMediator : Mediator
    {
        public SpeechActivity activity;
        public GameObject scroll;
        public GameObject card_prefab;

        public SpeechActivityMediator(string mediator_name, SpeechActivity activity) : base(mediator_name: mediator_name, component: activity)
        {
            this.activity = activity;
            this.scroll = activity.scroll;
            this.card_prefab = activity.card_prefab;
        }

        public override ENotification[] registerNotifications()
        {
            return NO_TIFICATION;
        }

        public override void onNotificationListener(INotification notification)
        {

        }

        public override void onRegister()
        {
            AppFacade.getInstance().registerCommand(ENotification.InitSpeech, () =>
            {
                return new InitSpeechCommand();
            });

            SpeechNorm norm = new SpeechNorm(mediator_name: vts.MediatorName.SpeechActivity,
                                             scroll: scroll,
                                             proxy_name: ProxyName.VocabularyProxy,
                                             target: Config.target,
                                             describe: Config.describe,
                                             table_name: "table1");

            AppFacade.getInstance().sendNotification(ENotification.InitSpeech, body: norm);
        }

        public override void onRemove()
        {

        }
    }
}
