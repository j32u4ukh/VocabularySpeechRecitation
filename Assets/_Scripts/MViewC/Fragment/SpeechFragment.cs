using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityMVC;

namespace VTS
{
    public class SpeechFragment : Mediator
    {
        [SerializeField] private Transform content;
        [SerializeField] private GameObject prefab;
        private ScrollRect scroll;

        private List<Button> buttons;
        private int n_card;

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
        [SerializeField] private float card_size;

        // 卡片間空隙
        [SerializeField] private float spacing;

        // 當前卡片索引值
        protected int card_index;
        #endregion

#if DEBUG
        [SerializeField] private Vector2 left;
        [SerializeField] private Vector2 right;
#endif

        private void Start()
        {
            scroll = GetComponent<ScrollRect>();
            content_rt = content.GetComponent<RectTransform>();
            FIXED_WAITING_TIME = new WaitForSeconds(FIXED_TIME);

            ScrollViewHandler handler = GetComponent<ScrollViewHandler>();
            handler.onPointerDown.AddListener(onPointerDownListener);
            handler.onEndDrag.AddListener(onEndDragListener);
        }

        private void OnDisable()
        {
            // TODO: 釋放資源
        }

#if DEBUG
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            left = new Vector2(0, scroll.transform.position.y);
            right = new Vector2(1080, scroll.transform.position.y);
            Gizmos.DrawLine(left, right);
        }
#endif

        public override IEnumerable<string> subscribeNotifications()
        {
            return new string[]
            {
                Notification.GroupLoaded,
                Notification.FinishedReading
            };
        }

        public override void onNotificationListener(INotification notification)
        {
            switch (notification.getName())
            {
                case Notification.GroupLoaded:
                    init();
                    break;

                // 當前單字念完
                case Notification.FinishedReading:
                    Facade.getInstance().sendNotification(Notification.NextVocabulary);
                    break;
            }
        }

        void onPointerDownListener(PointerEventData data)
        {
            Utils.log();
            SpeechManager.getInstance().stop();
            SpeechManager.getInstance().release(); 
            Facade.getInstance().sendNotification(Notification.StopSpeaking);
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
            while (Mathf.Abs(scroll.velocity.y) > MIN_SPEED)
            {
                alignCard(last_call: false);
                yield return FIXED_WAITING_TIME;
            }

            alignCard(last_call: true);
        }

        /// <summary>
        /// 對齊卡片中線，最後一次呼叫時，會先將速度歸零，再調整位置
        /// </summary>
        /// <param name="last_call"></param>
        public void alignCard(bool last_call = true)
        {
            Vector2 position = content_rt.anchoredPosition;

            (int index, float position) closest_card = getClosestCard(position.y);
            setCardIndex(index: closest_card.index);
            float target = closest_card.position;

            if (last_call)
            {
                // 速度歸零
                scroll.StopMovement();

                // 設定位置
                position.y = target;
            }
            else
            {
                // 每次經過卡片都額外減速
                scroll.velocity *= 0.9f;

                // 設定位置
                position.y = Mathf.Lerp(position.y, target, Mathf.Clamp01(ALIGN_SPEED * Time.deltaTime));
            }

            // 調整位置，速度若不為零，仍會繼續滑動，但應該可以造成切換卡片時的卡頓感
            content_rt.anchoredPosition = position;
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

            for (i = 0; i < len; i++)
            {
                position = positions[i];

                if (current_position <= position + offset)
                {
                    break;
                }
            }

            return (i, position);
        }

        /// <summary>
        /// 更新目前指向的卡片的索引值，並更新卡片視覺回饋
        /// </summary>
        /// <param name="index"></param>
        public void setCardIndex(int index)
        {
            card_index = index;

            // TODO: 卡片被選取時，應有視覺回饋
        }

        public IEnumerator alignCardCoroutine(int index)
        {
            float destinestion = positions[index];
            float max = 10f;

            for (float t = 0f; t < max; t++)
            {
                alignCard(destinestion: destinestion, rate: t / max, last_call: false);
                yield return FIXED_WAITING_TIME;
            }

            alignCard(destinestion: destinestion, rate: 1f, last_call: true);
        }

        void alignCard(float destinestion, float rate, bool last_call = true)
        {
            Vector2 position = content_rt.anchoredPosition;

            if (last_call)
            {
                // 設定位置
                position.y = destinestion;
            }
            else
            {
                // 設定位置
                position.y = Mathf.Lerp(a: position.y, b: destinestion, t: rate);
            }

            content_rt.anchoredPosition = position;
        }

        public int getCardIndex()
        {
            return card_index;
        }

        public int getCardNumber()
        {
            return n_card;
        }

        public Button getCard(int index)
        {
            if (index < 0 || getCardNumber() <= index)
            {
                throw new IndexOutOfRangeException(message: $"Index: {index} out of range(0 ~ {getCardNumber() - 1})");
            }

            return buttons[index];
        }

        /// <summary>
        /// 根據 GroupProxy 初始化 SpeechActivity
        /// </summary>
        void init()
        {
            if (!Facade.getInstance().tryGetProxy(out GroupProxy group))
            {
                Utils.error($"GroupProxy is not exists.");
                return;
            }
            
            buttons = new List<Button>();
            positions = new List<float>();
            card_index = 0;

            int c, n_children = content.childCount - 1;

            for(c = 1; c < n_children; c++)
            {
                Destroy(content.GetChild(index: c).gameObject);
            }

            n_card = group.getRowNumber();
            Utils.log($"vocab: {n_card}");

            string source = group.getSource();
            (string vocabulary, string description) row;
            GameObject obj;
            Button button;
            float position = 0;

            for (int i = n_card - 1; i >= 0; i--)
            {
                row = group.getRow(row_index: i);
                VocabularyNorm norm = new VocabularyNorm(vocabulary: row.vocabulary, description: row.description);

                obj = Instantiate(original: prefab, parent: content);
                obj.transform.SetSiblingIndex(1);
                button = obj.GetComponent<Button>();

                // 紀錄當前卡片位置
                positions.Add(position);
                position += (card_size + spacing);

                // 按鈕呈現文字，說明當前是哪個單字
                obj.GetComponentInChildren<Text>().text = norm.ToString();

                button.onClick.AddListener(() =>
                {
                    Facade.getInstance().sendNotification(Notification.Speak, data: norm);
                });

                buttons.Add(button);
            }

            buttons.Reverse();
        }
    }
}