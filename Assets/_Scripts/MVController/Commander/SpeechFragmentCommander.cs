using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVC;

namespace VTS
{
    public class SpeechFragmentCommander : Commander
    {
        [SerializeField] SpeechFragment fragment;

        private void OnEnable()
        {
            fragment.alignCard(index: 0);
        }

        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[] {
                Notification.NextVocabulary
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.NextVocabulary:
                    nextVocabulary();
                    break;
            }
        }

        void nextVocabulary()
        {
            int card_index = fragment.getCardIndex();

            if (card_index + 1 < fragment.getCardNumber())
            {
                StartCoroutine(nextVocabularyCoroutine(index: card_index + 1));
            }
            else
            {

            }
        }

        void alignCard(int index)
        {
            StartCoroutine(fragment.alignCardCoroutine(index: index));
        }

        IEnumerator nextVocabularyCoroutine(int index)
        {
            yield return StartCoroutine(fragment.alignCardCoroutine(index: index));

            // 觸發目標卡片以念誦單字
            fragment.getCard(index: index).onClick.Invoke();
        }
    }
}