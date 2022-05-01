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

        public GameObject speech_bookmark;
        public GameObject custom_bookmark;
        public GameObject exam_bookmark;
        public GameObject setting_bookmark;

        Transform[] bookmarks;
        int BOOKMARK_SIZE = 4;
        
        private void Start()
        {
            // 主畫面上方，頁籤區域
            AppFacade.getInstance().registerMediator(new BookmarkFragmentMediator(mediator_name: vts.MediatorName.BookmarkFragment,
                                                                                  component: gameObject));

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
