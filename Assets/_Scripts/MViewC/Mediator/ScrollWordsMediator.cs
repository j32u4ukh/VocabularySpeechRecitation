using PureMVC.Interfaces;
using SpeechLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using vts.mvc;

namespace vts
{

    public class ScrollWordsMediator : Mediator
    {
        Transform content;

        public ScrollWordsMediator(string mediator_name, Transform content) : base(mediator_name: mediator_name, component: content)
        {
            Utils.log($"MediatorName: {MediatorName}");
            this.content = content;
        }

        public override ENotification[] registerNotifications()
        {
            return new ENotification[] { 
                ENotification.VocabularyLoaded
            };
        }

        public override void handleNotification(INotification notification)
        {
            Utils.log($"Handle notification.Name: {notification.Name}");

            ENotification en = (ENotification)Enum.Parse(typeof(ENotification), notification.Name);
            Utils.log($"Handle notification: {en}");

            switch (en)
            {
                case ENotification.VocabularyLoaded:
                    loadVocabulary();
                    break;
            }
        }

        public override void onRegister()
        {

        }

        public override void onRemove()
        {

        }

        void loadVocabulary()
        {
            VocabularyProxy vocabulary_proxy = AppFacade.getInstance().getProxy(proxy_name: ProxyName.VocabularyProxy) as VocabularyProxy;
            SystemLanguage target = vocabulary_proxy.getLanguage(), describe = SystemLanguage.ChineseTraditional;
            Button button;

            for (int i = 0; i < content.childCount; i++)
            {
                button = content.GetChild(index: i).GetComponent<Button>();
                VocabularyNorm vocab = vocabulary_proxy.getVocabulary(index: i);
                Utils.log($"Setting button({i}) vocabulary: {vocab.vocabulary}, description: {vocab.description}");

                button.onClick.AddListener(()=> 
                {
                    SpeechManager.getInstance().startReciteContent(vocab: vocab, target: target, describe: describe);
                });
            }
        }
    }

}