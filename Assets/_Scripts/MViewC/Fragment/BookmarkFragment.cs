using UnityEngine;
using UnityEngine.EventSystems;
using vts.mvc;

namespace vts
{
    public class BookmarkFragment : MonoBehaviour, IPointerDownHandler
    {
        #region 目前使用 Fragment 的物件名稱，可協助外部判斷是否有 切換 Bookmark，還是同一個 Bookmark 重複在點
        [HideInInspector] public const string speech = "SpeechFragment";
        [HideInInspector] public const string custom = "CustomFragment";
        [HideInInspector] public const string exam = "ExamFragment";
        [HideInInspector] public const string setting = "SettingFragment"; 
        #endregion

        public GameObject speech_obj;
        public GameObject custom_obj;
        public GameObject exam_obj;
        public GameObject setting_obj;

        Transform[] bookmarks;
        int BOOKMARK_SIZE = 4;
        
        private void Start()
        {
            bookmarks = new Transform[] {
                speech_obj.transform,
                custom_obj.transform,
                exam_obj.transform,
                setting_obj.transform
            };
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            string bookmark = string.Empty;

            for (int i = 0; i < BOOKMARK_SIZE; i++)
            {
                if (eventData.pointerCurrentRaycast.gameObject.transform.IsChildOf(bookmarks[i]))
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

                    AppFacade.getInstance().sendNotification(ENotification.SwitchBookmark, type: bookmark);
                    break;
                }
            }
        }
    }
}
