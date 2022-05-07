using System.Collections.Generic;
using UnityMVC;

namespace VTS
{
    public class SpeechFragmentCommander : Commander
    {
        SpeechFragment fragment;

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
            Utils.log($"card_index: {card_index}, n_card: {fragment.getCardNumber()}");

            if (card_index + 1 < fragment.getCardNumber())
            {
                card_index++;
                Utils.log($"Next card_index: {card_index}");

                fragment.setCardIndex(index: card_index);
                Utils.log($"Curent card_index: {fragment.getCardIndex()}");

                //fragment.alignCard();
                fragment.getCard(index: card_index).onClick.Invoke();
            }
            else
            {

            }
        }
    }
}