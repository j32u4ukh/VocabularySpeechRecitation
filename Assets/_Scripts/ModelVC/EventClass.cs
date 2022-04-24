using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace vts
{
    public class StringEvent : UnityEvent<string> { }

    public class Vector2Event : UnityEvent<Vector2> { }

    public class PointerEventEvent : UnityEvent<PointerEventData> { }
}
