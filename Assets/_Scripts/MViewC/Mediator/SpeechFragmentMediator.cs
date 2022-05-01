using PureMVC.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace vts.mvc
{
    public class SpeechFragmentMediator : Mediator
    {
        SpeechFragment fragment;
        public Transform content;
        public GameObject prefab;

        public static Action<GameObject, Transform> onCreateGroup;

        public SpeechFragmentMediator(string mediator_name, SpeechFragment fragment) : base(mediator_name: mediator_name, component: fragment)
        {
            this.fragment = fragment;
            content = fragment.content;
            prefab = fragment.prefab;
        }

        public override ENotification[] registerNotifications()
        {
            return new ENotification[] { ENotification.GroupListLoaded };
        }

        public override void onNotificationListener(INotification notification)
        {
            ENotification en = AppFacade.transNameToEnum(name: notification.Name);

            switch (en)
            {
                case ENotification.GroupListLoaded:
                    displayGroupList();
                    break;
            }
        }

        #region MyRegion
        public override void onRegister()
        {
            Utils.log();
        }

        public override void onRemove()
        {

        } 
        #endregion

        void displayGroupList()
        {
            Utils.log();

            //foreach (Transform child in content)
            //{
            //    GameManager.getInstance().destory(child);
            //}

            SpeechFragmentProxy proxy = AppFacade.getInstance().getProxy(proxy_name: vts.ProxyName.GroupList) as SpeechFragmentProxy;
            Utils.log($"#group: {proxy.table.getRowNumber()}");
            GameObject obj;
            Button button;

            foreach (List<string> row in proxy.table.iterTable())
            {
                Utils.log(row.toString());

                onCreateGroup?.Invoke(prefab, content);
                //obj = GameManager.getInstance().getInstantiate(prefab: prefab, parent: content);
                //button = obj.GetComponent<Button>();

                //// 按鈕呈現文字，說明當前為哪個單字組(Group)
                //obj.GetComponent<Text>().text = row[0];
                //Utils.log(row[0]);

                //button.onClick.AddListener(() =>
                //{
                //    //SpeechNorm norm = new SpeechNorm(mediator_name: vts.MediatorName.SpeechActivity,
                //    //                                 scroll: scroll.gameObject,
                //    //                                 proxy_name: ProxyName.VocabularyProxy,
                //    //                                 target: SystemLanguage.English,
                //    //                                 describe: SystemLanguage.Chinese,
                //    //                                 table_name: "table1");

                //    //AppFacade.getInstance().sendNotification(ENotification.InitSpeech, body: norm);
                //});
            }
        }
    }
}
