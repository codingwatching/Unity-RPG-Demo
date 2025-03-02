using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public abstract class BasePanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public bool isOpened { get; set; }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
    }
}