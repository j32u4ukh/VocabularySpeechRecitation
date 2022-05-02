using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityMVC;

namespace VTS
{
    public class ScrollWordsMediator : Mediator
    {
        private ScrollRect scroll;
        private Transform content;
        private string proxy_name;

        List<Button> buttons;
        int n_card;

        #region 中線對齊相關
        private RectTransform content_rt;

        // 對齊等待時間物件
        private WaitForSeconds FIXED_WAITING_TIME;

        // 各個卡片在中線時，content 的位置
        private List<float> positions;

        // 多久呼叫一次對齊
        private float FIXED_TIME = 0.02f;

        // 小於此速度，強制定位到當前卡片的中線
        private float MIN_SPEED = 3.0f;

        // 對齊速度
        private float ALIGN_SPEED = 10.0f;

        // 卡片尺寸
        private float card_size;

        // 卡片間空隙
        private float spacing;

        // 當前卡片索引值
        private int card_index;
        #endregion

        private void Start()
        {
            ScrollViewHandler handler = scroll.GetComponent<ScrollViewHandler>();
            handler.onEndDrag.AddListener(onEndDragListener);
        }

        //public ScrollWordsMediator()
        //{
        //    this.scroll = scroll.GetComponent<ScrollRect>();
        //    this.proxy_name = proxy_name;

        //    //
        //    this.content = this.scroll.content.transform;
        //    this.content_rt = content.GetComponent<RectTransform>();
        //    VerticalLayoutGroup layout = this.content.GetComponent<VerticalLayoutGroup>();

        //    FIXED_WAITING_TIME = new WaitForSeconds(FIXED_TIME);
        //    positions = new List<float>();
        //    card_size = content.GetChild(index: 1).GetComponent<RectTransform>().rect.height;
        //    spacing = layout.spacing;
        //    card_index = 0;

        //    //
        //}



        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[] 
            {
                Notification.VocabularyLoaded,
                Notification.FinishedReading
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.VocabularyLoaded:
                    loadVocabulary();
                    break;

                // 當前單字念完
                case Notification.FinishedReading:
                    nextVocabulary();
                    break;
            }
        }

        void onEndDragListener(PointerEventData data)
        {
            StartCoroutine(alignCardCoroutine());
        }

        /// <summary>
        /// 校正 Card 到中線位置
        /// </summary>
        /// <returns></returns>
        IEnumerator alignCardCoroutine()
        {
            while(Mathf.Abs(scroll.velocity.y) > MIN_SPEED)
            {
                alignCard();
                yield return FIXED_WAITING_TIME;
            }

            Utils.log($"Call alignCard after scrool stop, velocity: {scroll.velocity.y}");
            alignCard(last_call: true);
        }

        /// <summary>
        /// 對齊卡片中線，最後一次呼叫時，會先將速度歸零，再調整位置
        /// </summary>
        /// <param name="last_call"></param>
        void alignCard(bool last_call = false)
        {
            Vector2 position = content_rt.anchoredPosition;

            (int index, float position) closest_card = getClosestCard(position.y);
            setCardIndex(index: closest_card.index);
            float target = closest_card.position;
            Utils.log($"target: {target}");

            if (last_call)
            {
                // 速度歸零
                scroll.StopMovement();
                position.y = target;
            }
            else
            {
                position.y = Mathf.Lerp(position.y, target, Mathf.Clamp01(ALIGN_SPEED * Time.deltaTime));
            }

            // 調整位置，速度若不為零，仍會繼續滑動，但應該可以造成切換卡片時的卡頓感
            content_rt.anchoredPosition = position;

            // 每次經過卡片都額外減速
            scroll.velocity *= 0.9f;
        }

        /// <summary>
        /// 取得當前位置最接近的卡片索引值與其位置
        /// </summary>
        /// <param name="current_position">當前位置</param>
        /// <returns>最接近的卡片索引值與其位置</returns>
        (int index, float position) getClosestCard(float current_position)
        {
            int i, len = positions.Count;
            float position = 0f, offset = (card_size + spacing) / 2.0f;

            for(i = 0; i < len; i++)
            {
                position = positions[i];

                if (current_position <= position + offset)
                {
                    Utils.log($"Closest is card {i}");
                    break;
                }
            }

            return (i, position);
        }

        /// <summary>
        /// 更新目前指向的卡片的索引值，並更新卡片視覺回饋
        /// </summary>
        /// <param name="index"></param>
        void setCardIndex(int index)
        {
            card_index = index;

            // TODO: 卡片被選取時，應有視覺回饋
        }

        // TODO: 根據 VocabularyProxy 生成列表，並更新 content 的高度(單字列表 + 上下空白區域)
        void loadVocabulary()
        {
            //VocabularyProxy vocabulary_proxy = AppFacade.getInstance().getProxy(proxy_name: proxy_name) as VocabularyProxy;
            //buttons = new List<Button>();
            //VocabularyNorm vocab;
            //Transform child;
            //Text label;
            //Button button;

            //int i, index;
            //n_card = content.childCount - 2;
            //float position = 0;

            //for (i = 0; i < n_card; i++)
            //{
            //    index = i + 1;
            //    child = content.GetChild(index: index);
            //    positions.Add(position);
            //    Utils.log($"Card {index} position: {position}");

            //    position += (card_size + spacing);

            //    SpeakNorm norm = new SpeakNorm(proxy_name: proxy_name, index: index);

            //    vocab = vocabulary_proxy.getVocabulary(index: index);
            //    label = child.GetComponentInChildren(typeof(Text)) as Text;
            //    label.text = $"{vocab.vocabulary} {vocab.description}";

            //    button = child.GetComponent<Button>();

            //    button.onClick.AddListener(()=> 
            //    {
            //        // TODO: 將要使用的語言隨著 vocab 一起傳過去
            //        AppFacade.getInstance().sendNotification(ENotification.Speak, body: norm);
            //    });

            //    buttons.Add(button);
            //}
        }

        void nextVocabulary()
        {
            Utils.log($"card_index: {card_index}, n_card: {n_card}");
            
            if(card_index + 1 < n_card)
            {
                card_index++;
                buttons[card_index].onClick.Invoke();
            }
            else
            {

            }
        }
    }

}