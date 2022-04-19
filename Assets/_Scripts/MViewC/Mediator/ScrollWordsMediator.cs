using PureMVC.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace vts.mvc
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
            AppFacade.getInstance().registerCommand(ENotification.Speak, () =>
            {
                return new SpeakCommand();
            });
        }

        public override void onRemove()
        {

        }

        void loadVocabulary()
        {
            VocabularyProxy vocabulary_proxy = AppFacade.getInstance().getProxy(proxy_name: ProxyName.VocabularyProxy) as VocabularyProxy;
            SystemLanguage target = vocabulary_proxy.getLanguage(), describe = SystemLanguage.ChineseTraditional;
            Transform child;
            Text label;
            Button button;

            for (int i = 0; i < content.childCount; i++)
            {
                child = content.GetChild(index: i);
                VocabularyNorm vocab = vocabulary_proxy.getVocabulary(index: i);
                label = child.GetComponentInChildren(typeof(Text)) as Text;
                label.text = $"{vocab.vocabulary} {vocab.description}";

                Utils.log($"Setting button({i}) vocabulary: {vocab.vocabulary}, description: {vocab.description}");
                button = child.GetComponent<Button>();

                button.onClick.AddListener(()=> 
                {
                    // TODO: 將要使用的語言隨著 vocab 一起傳過去
                    AppFacade.getInstance().sendNotification(ENotification.Speak, body: vocab);
                });
            }
        }
    }

}