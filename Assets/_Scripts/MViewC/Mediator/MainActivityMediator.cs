using PureMVC.Interfaces;
using System;
using UnityEngine;

namespace vts.mvc
{
    public class MainActivityMediator : Mediator
    {
        MainActivity main;
        GameObject speech_obj;
        GameObject custom_obj;
        GameObject exam_obj;
        GameObject setting_obj;
        GameObject current;

        public MainActivityMediator(string mediator_name, GameObject activity) : base(mediator_name: mediator_name, component: activity)
        {
            main = activity.GetComponent<MainActivity>();
            speech_obj = main.speech_fragment;
            custom_obj = main.custom_fragment;
            exam_obj = main.exam_fragment;
            setting_obj = main.setting_fragment;

            current = speech_obj;
        }

        public override ENotification[] registerNotifications()
        {
            return new ENotification[] { ENotification.SwitchBookmark };
        }

        public override void onNotificationListener(INotification notification)
        {
            ENotification en = AppFacade.transNameToEnum(name: notification.Name);

            // 根據通知的名稱作相應的處理
            switch (en)
            {
                case ENotification.SwitchBookmark:
                    switchBookmark(bookmark: notification.Type);
                    break;
            }
        }

        #region Life cycle
        public override void onRegister()
        {
            Utils.log();

            // 主畫面上方，頁籤區域
            AppFacade.getInstance().registerMediator(new BookmarkFragmentMediator(mediator_name: vts.MediatorName.BookmarkFragment,
                                                                                  component: main.bookmark_fragment));

            // TODO: 初始化數據庫列表，初始化完成再初始化 vts.MediatorName.SpeechFragment
            Utils.log("TODO: 初始化數據庫列表");

            AppFacade.getInstance().registerProxy(proxy: new SpeechFragmentProxy(proxy_name: vts.ProxyName.SpeechFragment));

            // 主畫面下方，初始頁面
            AppFacade.getInstance().registerMediator(new SpeechFragmentMediator(mediator_name: vts.MediatorName.SpeechFragment,
                                                                                component: main.speech_fragment));
        }

        public override void onRemove()
        {

        } 
        #endregion

        void switchBookmark(string bookmark)
        {
            // 若點選的和當前頁籤相同，則無需切換
            if (current.name.Equals(bookmark))
            {
                return;
            }

            Utils.log($"{current.name} -> {bookmark}");
            current.SetActive(false);

            switch (bookmark)
            {
                case BookmarkFragment.speech:
                    current = speech_obj;
                    break;
                case BookmarkFragment.custom:
                    current = custom_obj;
                    break;
                case BookmarkFragment.exam:
                    current = exam_obj;
                    break;
                case BookmarkFragment.setting:
                    current = setting_obj;
                    break;
            }

            current.SetActive(true);
        }
    }
}


