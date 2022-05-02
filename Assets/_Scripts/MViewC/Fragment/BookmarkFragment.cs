using UnityEngine;
using UnityEngine.EventSystems;
using UnityMVC;

namespace VTS
{
    public class BookmarkFragment : Mediator, IPointerDownHandler
    {
        #region 目前使用 Fragment 的物件名稱，可協助外部判斷是否有 切換 Bookmark，還是同一個 Bookmark 重複在點
        [HideInInspector] public const string speech = "SpeechFragment";
        [HideInInspector] public const string custom = "CustomFragment";
        [HideInInspector] public const string exam = "ExamFragment";
        [HideInInspector] public const string setting = "SettingFragment"; 
        #endregion

        public GameObject speech_bookmark;
        public GameObject custom_bookmark;
        public GameObject exam_bookmark;
        public GameObject setting_bookmark;

        Transform[] bookmarks;
        int BOOKMARK_SIZE = 4;
        
        private void Start()
        {
            bookmarks = new Transform[] {
                speech_bookmark.transform,
                custom_bookmark.transform,
                exam_bookmark.transform,
                setting_bookmark.transform
            };
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            string bookmark = string.Empty;
            Transform current = eventData.pointerCurrentRaycast.gameObject.transform;

            for (int i = 0; i < BOOKMARK_SIZE; i++)
            {
                if (current.IsChildOf(bookmarks[i]))
                {
                    switch (i)
                    {
                        case 0:
                            bookmark = speech;
                            break;
                        case 1:
                            bookmark = custom;
                            break;
                        case 2:
                            bookmark = exam;
                            break;
                        case 3:
                            bookmark = setting;
                            break;
                    }

                    Facade.getInstance().sendNotification(Notification.SwitchBookmark, header: bookmark);
                    break;
                }
            }
        }
    }
}
