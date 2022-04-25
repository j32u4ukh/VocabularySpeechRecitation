using UnityEngine;
using UnityEngine.EventSystems;

namespace vts
{
    public class BookmarkFragment : MonoBehaviour, IPointerDownHandler
    {
        public GameObject vocabulary_obj;
        public GameObject custom_obj;
        public GameObject setting_obj;
        public GameObject other_obj;

        Transform[] bookmarks;
        int BOOKMARK_SIZE = 4;
        
        private void Start()
        {
            bookmarks = new Transform[] {
                vocabulary_obj.transform,
                custom_obj.transform,
                setting_obj.transform,
                other_obj.transform
            };
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            for (int i = 0; i < BOOKMARK_SIZE; i++)
            {

                if (eventData.pointerCurrentRaycast.gameObject.transform.IsChildOf(bookmarks[i]))
                {
                    Utils.log(bookmarks[i].name);
                    break;
                }
            }
        }
    }
}
