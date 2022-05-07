using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VTS
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollViewHandler : MonoBehaviour, IInitializePotentialDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, 
                                     IScrollHandler, ICanvasElement, ILayoutElement, ILayoutGroup, ILayoutController
    {
        public PointerEventEvent onPointerDown = new PointerEventEvent();
        public PointerEventEvent onDrag = new PointerEventEvent();
        public PointerEventEvent onScroll = new PointerEventEvent();
        public PointerEventEvent onEndDrag = new PointerEventEvent();

        public float minWidth => scroll == null ? -1 : scroll.minWidth;

        public float preferredWidth => scroll == null ? -1 : scroll.preferredWidth;

        public float flexibleWidth => scroll == null ? -1 : scroll.flexibleWidth;

        public float minHeight => scroll == null ? -1 : scroll.minHeight;

        public float preferredHeight => scroll == null ? -1 : scroll.preferredHeight;

        public float flexibleHeight => scroll == null ? -1 : scroll.flexibleHeight;

        public int layoutPriority => scroll == null ? -1 : scroll.layoutPriority;

        private ScrollRect scroll = null;

        private void Start()
        {
            scroll = GetComponentInParent<ScrollRect>();
        }

        #region 拖曳事件
        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            onPointerDown.Invoke(eventData);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnDrag(PointerEventData eventData)
        {
            onDrag.Invoke(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            onScroll.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag.Invoke(eventData);
        } 
        #endregion

        public void SetLayoutHorizontal()
        {
            
        }

        public void SetLayoutVertical()
        {
            
        }

        public void Rebuild(CanvasUpdate executing)
        {
            
        }

        public void LayoutComplete()
        {
            
        }

        public void GraphicUpdateComplete()
        {
            
        }

        public bool IsDestroyed()
        {
            return scroll == null ? false : scroll.IsDestroyed();
        }

        public void CalculateLayoutInputHorizontal()
        {
            
        }

        public void CalculateLayoutInputVertical()
        {
            
        }
    }
}
