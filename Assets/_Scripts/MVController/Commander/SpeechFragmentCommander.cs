using System.Collections;
using System.Collections.Generic;
using UnityMVC;

namespace VTS
{
    public class SpeechFragmentCommander : Commander
    {
        SpeechFragment fragment;
        bool keep_speaking = true;

        private void Start()
        {
            fragment = GetComponent<SpeechFragment>();
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

        IEnumerator nextVocabularyCoroutine(int index)
        {
            // 設置目標卡片索引值
            fragment.setCardIndex(index: index);

            // 等待目標卡片移到中線位置
            yield return StartCoroutine(fragment.alignCardCoroutine(index: index));

            // 觸發目標卡片以念誦單字
            fragment.getCard(index: index).onClick.Invoke();
        }
    }
}