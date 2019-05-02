using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardUI
{
    public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public delegate void PointerBehaviour();
        public event PointerBehaviour OnPointerEnterBehaviour;
        public event PointerBehaviour OnPointerExitBehaviour;
        public event PointerBehaviour OnCardDrop;

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("Dropped");
            if(OnCardDrop != null)
            {
                OnCardDrop();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (OnPointerEnterBehaviour != null)
            {
                OnPointerEnterBehaviour();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(OnPointerExitBehaviour != null)
            {
                OnPointerExitBehaviour();
            }
        }
    }
}
