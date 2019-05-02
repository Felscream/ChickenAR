using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIAnimation))]
public class CancelCardSelection : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void PointerBehaviour();
    public event PointerBehaviour OnCardDrop;
    public event PointerBehaviour OnCardEnter;
    public event PointerBehaviour OnCardExit;
    private UIAnimation _animation;

    private void Awake()
    {
        _animation = GetComponent<UIAnimation>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        _animation.PlayAnimation();
        if (OnCardEnter != null)
        {
            OnCardEnter();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animation.PlayAnimation();
        if (OnCardExit != null)
        {
            OnCardExit();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(OnCardDrop != null)
        {
            OnCardDrop();
        }
    }
}
