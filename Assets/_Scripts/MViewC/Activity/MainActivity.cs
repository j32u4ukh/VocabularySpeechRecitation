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
        }

        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[] { 
                Notification.SwitchBookmark
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            // �ھڳq�����W�٧@�������B�z
            switch (notification.getName())
            {
                case Notification.SwitchBookmark:
                    switchBookmark(bookmark: notification.getHeader());
                    break;
            }
        }

        void switchBookmark(string bookmark)
        {
            // �Y�I�諸�M��e���ҬۦP�A�h�L�ݤ���
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
