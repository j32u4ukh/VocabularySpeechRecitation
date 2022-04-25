using PureMVC.Interfaces;
using UnityEngine;

namespace vts.mvc
{
    public class MainActivityMediator : Mediator
    {
        MainActivity main;

        public MainActivityMediator(string mediator_name, GameObject activity) : base(mediator_name: mediator_name, component: activity)
        {
            main = activity.GetComponent<MainActivity>();
        }

        public override ENotification[] registerNotifications()
        {
            return NO_TIFICATION;
        }

        public override void handleNotification(INotification notification)
        {
            // 根據通知的名稱作相應的處理
            switch (notification.Name)
            {
                default:
                    break;
            }
        }

        public override void onRegister()
        {
            Utils.log();

            if (!AppFacade.getInstance().hasMediator(mediator_name: vts.MediatorName.BookmarkFragment))
            {
                BookmarkFragmentMediator bookmark = new BookmarkFragmentMediator(mediator_name: vts.MediatorName.BookmarkFragment, 
                                                                                 component: main.bookmark_fragment);
                AppFacade.getInstance().registerMediator(bookmark);
            }

            if (!AppFacade.getInstance().hasMediator(mediator_name: vts.MediatorName.SpeechFragment))
            {
                SpeechFragmentMediator speech = new SpeechFragmentMediator(mediator_name: vts.MediatorName.SpeechFragment,
                                                                           component: main.speech_fragment);
                AppFacade.getInstance().registerMediator(speech);
            }

            // TODO: 初始化數據庫列表
            Utils.log("TODO: 初始化數據庫列表");
        }

        public override void onRemove()
        {

        }
    }
}


