using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class MainActivity : Mediator
    {
        [Header("Fragment")]
        #region Fragment
        public GameObject bookmark_fragment;
        public GameObject speech_fragment;
        public GameObject custom_fragment;
        public GameObject exam_fragment;
        public GameObject setting_fragment;

        GameObject current;
        #endregion

        [Header("Activity")]
        public GameObject speech_activity;

        private void Start()
        {
            current = speech_fragment;

            // 要求載入單字組清單
            Facade.getInstance().sendNotification(Notification.InitGroupList);
        }

        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[] { 
                Notification.SwitchBookmark
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            // 根據通知的名稱作相應的處理
            switch (notification.getName())
            {
                case Notification.SwitchBookmark:
                    switchBookmark(bookmark: notification.getHeader());
                    break;
            }
        }

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
                    current = speech_fragment;
                    break;
                case BookmarkFragment.custom:
                    current = custom_fragment;
                    break;
                case BookmarkFragment.exam:
                    current = exam_fragment;
                    break;
                case BookmarkFragment.setting:
                    current = setting_fragment;
                    break;
            }

            current.SetActive(true);
        }
    }
}
